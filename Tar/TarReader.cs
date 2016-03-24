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
        private byte[] _buffer = new byte[2 * TarCommon.BlockSize];
        private long _remaining;
        private int _remainingPadding;

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
            public Dictionary<string, string> PaxAttributes;
        }

        private static string GetPaxValue(ref ReadState state, string paxKey)
        {
            string paxValue;
            if (paxKey != null && state.PaxAttributes != null && state.PaxAttributes.TryGetValue(paxKey, out paxValue))
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
                state.Offset += length;
                return paxValue;
            }

            var s = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var b = state.Buffer[state.Offset + i];
                if (b == 0)
                {
                    break;
                }
                else if (b > 127)
                {
                    throw new TarParseException("non-ASCII byte in legacy header");
                }
                else
                {
                    s.Append((char)b);
                }
            }

            state.Offset += length;
            return s.ToString();
        }

        static readonly char[] nulAndSpace = new char[]{' ', '\x00'};
        static readonly char[] nul = new char[]{'\x00'};

        private static long GetOctalLong(ref ReadState state, int length, string paxKey)
        {
            string paxValue = GetPaxValue(ref state, paxKey);
            if (paxValue != null)
            {
                state.Offset += length;
                return Convert.ToInt64(paxValue);
            }

            if ((state.Buffer[state.Offset] & 128) != 0)
            {
                ulong result = 0;
                if ((state.Buffer[state.Offset] & 64) != 0)
                {
                    // If the result is negative, then smear the sign bit left so that the final
                    // result is negative.
                    result = ~(ulong)0;
                }

                for (int i = 0; i < length; i++)
                {
                    byte b = state.Buffer[state.Offset + i];
                    if (i == 0)
                    {
                        b ^= 128;
                    }

                    result <<= 8;
                    result |= b;
                }

                state.Offset += length;
                return (long)result;
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
                throw new TarParseException("value too large");
            }

            return (int)value;
        }

        private static DateTime GetTime(ref ReadState state, int length, string paxKey)
        {
            string paxValue = GetPaxValue(ref state, paxKey);
            if (paxValue != null)
            {
                state.Offset += length;
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
                var nsStr = str.Substring(point + 1, Math.Min(9, str.Length - point - 1));
                if (nsStr.Length < 9)
                {
                    nsStr += new String('0', 9 - nsStr.Length);
                }

                nanoseconds = Convert.ToUInt32(nsStr, 10);
            }

            return TarTime.FromUnixTime(seconds, nanoseconds);
        }

        private static TarEntry ParseHeader(ref ReadState state)
        {
            var entry = new TarEntry();
            var initialOffset = state.Offset;

            entry.Name = GetString(ref state, 100, TarCommon.PaxPath);
            entry.Mode = GetOctal(ref state, 8, null);
            entry.UserID = GetOctal(ref state, 8, TarCommon.PaxUid);
            entry.GroupID = GetOctal(ref state, 8, TarCommon.PaxGid);
            entry.Length = GetOctalLong(ref state, 12, TarCommon.PaxSize);
            entry.ModifiedTime = GetTime(ref state, 12, TarCommon.PaxMtime);
            var checksum = GetOctal(ref state, 8, null);
            int signedChecksum;
            var unsignedChecksum = TarCommon.Checksum(state.Buffer, initialOffset, out signedChecksum);
            if (checksum != signedChecksum && checksum != unsignedChecksum)
            {
                throw new TarParseException("invalid tar checksum");
            }

            var typeFlag = state.Buffer[state.Offset];
            if (typeFlag == 0)
            {
                typeFlag = (byte)'0';
            }

            entry.Type = (TarEntryType)typeFlag;

            state.Offset++;
            entry.LinkTarget = GetString(ref state, 100, TarCommon.PaxLinkpath);
            var magic = GetString(ref state, 8, null);
            if (magic == TarCommon.PosixMagic || magic == TarCommon.GnuMagic)
            {
                entry.UserName = GetString(ref state, 32, TarCommon.PaxUname);
                entry.GroupName = GetString(ref state, 32, TarCommon.PaxGname);
                entry.DeviceMajor = GetOctal(ref state, 8, TarCommon.PaxDevmajor);
                entry.DeviceMinor = GetOctal(ref state, 8, TarCommon.PaxDevminor);
                if (magic == TarCommon.PosixMagic)
                {
                    if (state.PaxAttributes == null || !state.PaxAttributes.ContainsKey(TarCommon.PaxPath))
                    {
                        var prefix = GetString(ref state, 155, null);
                        if (prefix.Length > 0)
                        {
                            entry.Name = prefix + "/" + entry.Name;
                        }
                    }

                    string atime = GetPaxValue(ref state, TarCommon.PaxAtime);
                    if (atime != null)
                    {
                        entry.AccessTime = ParsePaxTime(atime);
                    }

                    string ctime = GetPaxValue(ref state, TarCommon.PaxCtime);
                    if (ctime != null)
                    {
                        entry.ChangeTime = ParsePaxTime(ctime);
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

            if (IsArrayZero(_buffer, padding, TarCommon.BlockSize))
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
                else if (IsArrayZero(_buffer, 0, TarCommon.BlockSize))
                {
                    return null;
                }
                else
                {
                    throw new TarParseException("non-zero header after zero header");
                }
            }

            var state = new ReadState
            {
                Buffer = _buffer,
                Offset = padding,
                PaxAttributes = paxAttributes,
            };

            var entry = ParseHeader(ref state);

            _remaining = entry.Length;
            _remainingPadding = TarCommon.Padding(_remaining);
            return entry;
        }

        public async Task<string> ReadGNUEntryData()
        {
            using (var reader = new StreamReader(CurrentFile, TarCommon.ASCII))
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
            var streamReader = new StreamReader(_currentStream, TarCommon.UTF8);
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