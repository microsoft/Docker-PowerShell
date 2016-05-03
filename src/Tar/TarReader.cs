using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tar
{
    public class TarReader
    {
        private readonly Stream _stream;
        private readonly TarReaderStream _currentEntryStream;
        private readonly byte[] _buffer = new byte[2 * TarCommon.BlockSize];
        private long _remaining;
        private int _remainingPadding;

        public TarReader(Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new NotSupportedException();
            }

            _stream = stream;
            _currentEntryStream = new TarReaderStream(this);
        }

        private static TarEntry ParseHeader(ref TarHeaderView header)
        {
            var entry = new TarEntry
            {
                Name = header.GetString(TarHeader.Name),
                Mode = header.GetOctal(TarHeader.Mode),
                UserID = header.GetOctal(TarHeader.UserID),
                GroupID = header.GetOctal(TarHeader.GroupID),
                Length = header.GetOctalLong(TarHeader.Length),
                ModifiedTime = header.GetTime(TarHeader.ModifiedTime)
            };

            var checksum = header.GetOctal(TarHeader.Checksum);
            int signedChecksum;
            var unsignedChecksum = TarCommon.Checksum(header.Field(TarHeader.FullHeader), out signedChecksum);
            if (checksum != signedChecksum && checksum != unsignedChecksum)
            {
                throw new TarParseException("invalid tar checksum");
            }

            var typeFlag = header[TarHeader.TypeFlag.Offset];
            if (typeFlag == 0)
            {
                typeFlag = (byte)'0';
            }

            entry.Type = (TarEntryType)typeFlag;

            entry.LinkTarget = header.GetString(TarHeader.LinkTarget);
            var magic = header.GetString(TarHeader.FullMagic);
            if (magic == TarCommon.PosixMagic || magic == TarCommon.GnuMagic)
            {
                entry.UserName = header.GetString(TarHeader.UserName);
                entry.GroupName = header.GetString(TarHeader.GroupName);
                entry.DeviceMajor = header.GetOctal(TarHeader.DeviceMajor);
                entry.DeviceMinor = header.GetOctal(TarHeader.DeviceMinor);
                if (magic == TarCommon.PosixMagic)
                {
                    if (header.PaxAttributes == null || !header.PaxAttributes.ContainsKey(TarHeader.Name.PaxAttribute))
                    {
                        var prefix = header.GetString(TarHeader.Prefix);
                        if (prefix.Length > 0)
                        {
                            entry.Name = prefix + "/" + entry.Name;
                        }
                    }

                    string atime = header.GetPaxValue(TarCommon.PaxAtime);
                    if (atime != null)
                    {
                        entry.AccessTime = TarTime.FromPaxTime(atime);
                    }

                    string ctime = header.GetPaxValue(TarCommon.PaxCtime);
                    if (ctime != null)
                    {
                        entry.ChangeTime = TarTime.FromPaxTime(ctime);
                    }

                    string creationtime = header.GetPaxValue(TarCommon.PaxCreationTime);
                    if (creationtime != null)
                    {
                        entry.CreationTime = TarTime.FromPaxTime(creationtime);
                    }

                    string fileAttributes = header.GetPaxValue(TarCommon.PaxWindowsFileAttributes);
                    if (fileAttributes != null)
                    {
                        entry.FileAttributes = (FileAttributes)Convert.ToUInt32(fileAttributes);
                    }

                    entry.SecurityDescriptor = header.GetPaxValue(TarCommon.PaxWindowsSecurityDescriptor);
                    if (header.GetPaxValue(TarCommon.PaxWindowsMountPoint) != null)
                    {
                        entry.IsMountPoint = true;
                    }
                }
            }

            return entry;
        }

        private async Task Skip(long count)
        {
            if (_stream.CanSeek)
            {
                _stream.Seek(count, SeekOrigin.Current);
            }
            else
            {
                var dummy = new byte[Math.Min(count, 65536)];
                for (long i = 0; i < count;)
                {
                    int skipped = await _stream.ReadAsync(dummy, 0, (int)Math.Min(dummy.Length, i - count));
                    if (skipped == 0)
                    {
                        throw new EndOfStreamException();
                    }

                    i += skipped;
                }
            }
        }

        private bool IsArrayZero(int offset, int length)
        {
            for (var i = 0; i < length; i++)
            {
                if (_buffer[offset + i] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public Stream CurrentFile => _currentEntryStream;

        private async Task<TarEntry> ReadHeader(Dictionary<string, string> paxAttributes)
        {
            if (_remaining > 0)
            {
                await Skip(_remaining);
                _remaining = 0;
            }

            var read = await _stream.ReadAsync(_buffer, 0, TarCommon.BlockSize + _remainingPadding);
            var padding = _remainingPadding;
            _remainingPadding = 0;
            if (read <= _remainingPadding)
            {
                // No more entries.
                return null;
            }
            else if (read - _remainingPadding < TarCommon.BlockSize)
            {
                throw new EndOfStreamException();
            }

            if (IsArrayZero(padding, TarCommon.BlockSize))
            {
                // Verify that the next block is also zero.
                read = await _stream.ReadAsync(_buffer, 0, TarCommon.BlockSize);
                if (read == 0)
                {
                    return null;
                }
                else if (read < TarCommon.BlockSize)
                {
                    throw new EndOfStreamException();
                }
                else if (IsArrayZero(0, TarCommon.BlockSize))
                {
                    return null;
                }
                else
                {
                    throw new TarParseException("non-zero header after zero header");
                }
            }

            var header = new TarHeaderView(_buffer, padding, paxAttributes);
            var entry = ParseHeader(ref header);

            _remaining = entry.Length;
            _remainingPadding = TarCommon.Padding(_remaining);
            return entry;
        }

        private async Task<string> ReadGNUEntryData()
        {
            using (var reader = new StreamReader(CurrentFile, TarCommon.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public async Task<TarEntry> GetNextEntryAsync()
        {
            var entry = await ReadHeader(null);
            if (entry == null)
            {
                return null;
            }

            string replacementName = null;
            string replacementLinkTarget = null;

            bool moreHeaders = true;
            for (;;)
            {
                switch (entry.Type)
                {
                    case TarCommon.PaxHeaderType:
                        var paxAttributes = await ReadPaxAttributes();
                        entry = await ReadHeader(paxAttributes);
                        if (entry == null)
                        {
                            throw new TarParseException("missing entry after PAX entry");
                        }

                        moreHeaders = false;
                        break;

                    case TarCommon.GnuLongPathnameType:
                        replacementName = await ReadGNUEntryData();
                        break;

                    case TarCommon.GnuLongLinknameType:
                        replacementLinkTarget = await ReadGNUEntryData();
                        break;

                    default:
                        moreHeaders = false;
                        break;
                }

                if (!moreHeaders)
                {
                    break;
                }

                entry = await ReadHeader(null);
            }

            if (replacementName != null)
            {
                entry.Name = replacementName;
            }

            if (replacementLinkTarget != null)
            {
                entry.LinkTarget = replacementLinkTarget;
            }

            switch (entry.Type)
            {
                case TarCommon.PaxHeaderType:
                    throw new TarParseException("extra PAX header");

                case TarCommon.GnuLongPathnameType:
                case TarCommon.GnuLongLinknameType:
                    throw new TarParseException("unexpected GNU entry type");

                case TarEntryType.File:
                case TarEntryType.HardLink:
                case TarEntryType.SymbolicLink:
                case TarEntryType.CharacterDevice:
                case TarEntryType.BlockDevice:
                case TarEntryType.Directory:
                case TarEntryType.FIFO:
                    break;

                default:
                    // Don't return enum values that this implementation doesn't understand.
                    entry.Type = TarEntryType.File;
                    break;
            }

            return entry;
        }

        private async Task<Dictionary<string, string>> ReadPaxAttributes()
        {
            var streamReader = new StreamReader(_currentEntryStream, TarCommon.UTF8);
            var values = new Dictionary<string, string>();
            for (;;)
            {
                var line = await streamReader.ReadLineAsync();
                if (line == null)
                {
                    break;
                }

                var space = line.IndexOf(' ');
                if (space < 0)
                {
                    throw new TarParseException("invalid PAX line");
                }

                var equals = line.IndexOf('=', space);
                if (equals < 0)
                {
                    throw new TarParseException("invalid PAX line");
                }

                var key = line.Substring(space + 1, equals - space - 1);
                var value = line.Substring(equals + 1);
                values.Add(key, value);
            }

            return values;
        }

        internal int ReadCurrentFile(byte[] buffer, int offset, int count)
        {
            if (count > _remaining)
            {
                count = (int)_remaining;
            }

            if (count == 0)
            {
                return 0;
            }

            int read = _stream.Read(buffer, offset, count);
            if (read == 0 && count > 0)
            {
                throw new EndOfStreamException();
            }

            _remaining -= read;
            return read;
        }

        internal async Task<int> ReadCurrentFileAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (count > _remaining)
            {
                count = (int)_remaining;
            }

            if (count == 0)
            {
                return 0;
            }

            int read = await _stream.ReadAsync(buffer, offset, count, cancellationToken);
            if (read == 0 && count > 0)
            {
                throw new EndOfStreamException();
            }

            _remaining -= read;
            return read;
        }
    }
}