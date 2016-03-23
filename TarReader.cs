using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tar
{
    public class TarReader
    {
        internal Stream _stream;
        internal TarReaderStream _currentStream;
        private byte[] _buffer = new byte[2 * TarUtils.BlockSize];
        private long _remaining;
        private int _remainingPadding;
        static private UTF8Encoding _utf8 = new UTF8Encoding(false);

        public TarReader(Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new NotSupportedException();
            }

            _stream = stream;
            _currentStream = new TarReaderStream(this);
        }

        private struct ReadState
        {
            public byte[] Buffer;
            public int Offset;
            public Dictionary<string, string> PaxEntries;
        }

        private static string GetPaxValue(ref ReadState state, string paxKey)
        {
            string paxValue;
            if (paxKey != null && state.PaxEntries != null && state.PaxEntries.TryGetValue(paxKey, out paxValue))
            {
                return paxValue;
            }

            return null;
        }

        private static string GetString(ref ReadState state, int length, string paxKey)
        {
            string paxValue = GetPaxValue(ref state, paxKey);
            if (paxValue != null)
            {
                return paxValue;
            }

            var s = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var b = state.Buffer[state.Offset + i];
                if (b == 0)
                {
                    return s.ToString();
                }
                else if (b > 127)
                {
                    throw new Exception("non-ASCII byte in legacy header");
                }
                else
                {
                    s.Append((char)b);
                }
            }

            throw new Exception("missing NUL terminator");
        }

        static readonly char[] nulAndSpace = new char[]{' ', '\x00'};
        static readonly char[] nul = new char[]{'\x00'};

        private static long GetOctalLong(ref ReadState state, int length, string paxKey)
        {
            string paxValue = GetPaxValue(ref state, paxKey);
            if (paxValue != null)
            {
                return Convert.ToInt64(paxValue);
            }

            if ((state.Buffer[state.Offset] & 128) != 0)
            {
                // TODO
                throw new NotSupportedException();
            }
            else
            {
                var str = GetString(ref state, length, null).Trim(nulAndSpace);
                if (str.Length == 0)
                {
                    return 0;
                }
                return Convert.ToInt64(str, 8);
            }
        }

        private static int GetOctal(ref ReadState state, int length, string paxKey)
        {
            long value = GetOctalLong(ref state, length, paxKey);
            if ((int)value != value)
            {
                throw new Exception("value too large");
            }

            return (int)value;
        }

        private static DateTime GetTime(ref ReadState state, int length, string paxKey)
        {
            string paxValue = GetPaxValue(ref state, paxKey);
            if (paxValue != null)
            {
                return ParsePaxTime(paxValue);
            }

            long unixTime = GetOctalLong(ref state, length, null);
            if (unixTime < TarTime.MinUnixTime)
            {
                unixTime = TarTime.MinUnixTime;
            }
            else if (unixTime > TarTime.MaxUnixTime)
            {
                unixTime = TarTime.MaxUnixTime;
            }

            return TarTime.FromUnixTime(unixTime, 0);
        }

        public static DateTime ParsePaxTime(string str)
        {
            var point = str.IndexOf('.');
            long seconds;
            uint nanoseconds;
            if (point < 0)
            {
                seconds = Convert.ToInt64(str, 10);
                nanoseconds = 0;
            }
            else
            {
                seconds = Convert.ToInt64(str.Substring(0, point), 10);
                var nsStr = str.Substring(point + 1, 9);
                if (nsStr.Length < 9)
                {
                    nsStr += new String('0', 9 - nsStr.Length);
                }

                nanoseconds = Convert.ToUInt32(nsStr, 10);
            }

            return TarTime.FromUnixTime(seconds, nanoseconds);
        }

        private static TarEntry ParseHeader(byte[] buffer, int offset, Dictionary<string, string> paxEntries)
        {
            var state = new ReadState
            {
                Buffer = buffer,
                Offset = offset,
                PaxEntries = paxEntries,
            };

            var entry = new TarEntry();

            entry.Name = GetString(ref state, 100, TarUtils.PaxPath);
            entry.Mode = GetOctal(ref state, 8, null);
            entry.UserID = GetOctal(ref state, 8, TarUtils.PaxUid);
            entry.GroupID = GetOctal(ref state, 8, TarUtils.PaxGid);
            entry.Length = GetOctalLong(ref state, 12, TarUtils.PaxSize);
            entry.ModifiedTime = GetTime(ref state, 12, TarUtils.PaxMtime);
            var checksum = GetOctal(ref state, 8, null);
            int signedChecksum;
            var unsignedChecksum = TarUtils.Checksum(buffer, offset, out signedChecksum);
            if (checksum != signedChecksum && checksum != unsignedChecksum)
            {
                throw new Exception("invalid tar checksum");
            }

            var typeFlag = state.Buffer[state.Offset];
            if (typeFlag == 0)
            {
                typeFlag = (byte)'0';
            }

            entry.Type = (TarEntryType)typeFlag;

            state.Offset++;
            entry.LinkTarget = GetString(ref state, 100, TarUtils.PaxLinkpath);
            var magic = GetString(ref state, 8, null);
            if (magic == TarUtils.PosixMagic || magic == TarUtils.GnuMagic)
            {
                // POSIX, star, or GNU format
                entry.UserName = GetString(ref state, 32, TarUtils.PaxUname);
                entry.GroupName = GetString(ref state, 32, TarUtils.PaxGname);
                entry.DeviceMajor = GetOctal(ref state, 8, TarUtils.PaxDevmajor);
                entry.DeviceMinor = GetOctal(ref state, 8, TarUtils.PaxDevminor);
                if (magic == TarUtils.PosixMagic && (state.PaxEntries == null || !state.PaxEntries.ContainsKey(TarUtils.PaxPath)))
                {
                    // POSIX or star
                    var prefix = GetString(ref state, 155, null);
                    if (prefix.Length > 0)
                    {
                        entry.Name = prefix + "/" + entry.Name;
                    }
                }

                string atime = GetPaxValue(ref state, TarUtils.PaxAtime);
                if (atime != null)
                {
                    entry.AccessTime = ParsePaxTime(atime);
                }

                string ctime = GetPaxValue(ref state, TarUtils.PaxCtime);
                if (ctime != null)
                {
                    entry.ChangeTime = ParsePaxTime(ctime);
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

        private bool IsArrayZero(byte[] buffer, int offset, int length)
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

        public Stream CurrentFile
        {
            get { return _currentStream; }
        }

        private async Task<TarEntry> ReadHeader(Dictionary<string, string> paxEntries)
        {
            if (_remaining > 0)
            {
                await Skip(_remaining);
                _remaining = 0;
            }

            var read = await _stream.ReadAsync(_buffer, 0, TarUtils.BlockSize + _remainingPadding);
            var padding = _remainingPadding;
            _remainingPadding = 0;
            if (read <= _remainingPadding)
            {
                // No more entries.
                return null;
            }
            else if (read - _remainingPadding < TarUtils.BlockSize)
            {
                throw new EndOfStreamException();
            }

            if (IsArrayZero(_buffer, padding, TarUtils.BlockSize))
            {
                // Verify that the next block is also zero.
                read = await _stream.ReadAsync(_buffer, 0, TarUtils.BlockSize);
                if (read == 0)
                {
                    return null;
                }
                else if (read < TarUtils.BlockSize)
                {
                    throw new EndOfStreamException();
                }
                else if (IsArrayZero(_buffer, 0, TarUtils.BlockSize))
                {
                    return null;
                }
                else
                {
                    throw new Exception("non-zero header after zero header");
                }
            }

            var entry = ParseHeader(_buffer, padding, paxEntries);

            _remaining = entry.Length;
            _remainingPadding = TarUtils.BlockSize - (int)(_remaining % TarUtils.BlockSize);
            if (_remainingPadding == TarUtils.BlockSize)
            {
                _remainingPadding = 0;
            }

            return entry;
        }

        public async Task<TarEntry> GetNextEntryAsync()
        {
            var entry = await ReadHeader(null);
            if (entry == null)
            {
                return null;
            }

            if (entry.Type == TarUtils.PaxHeaderType)
            {
                var paxEntries = await ReadPaxEntries();
                entry = await ReadHeader(paxEntries);
                if (entry == null)
                {
                    throw new Exception("missing entry after PAX entry");
                }
            }

            switch (entry.Type)
            {
            case TarUtils.PaxHeaderType:
                throw new Exception("extra PAX header");

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

        private async Task<Dictionary<string, string>> ReadPaxEntries()
        {
            var streamReader = new StreamReader(_currentStream, _utf8);
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
                    throw new Exception("invalid PAX line");
                }

                var equals = line.IndexOf('=', space);
                if (equals < 0)
                {
                    throw new Exception("invalid PAX line");
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