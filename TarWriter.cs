using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tar
{
    public class TarWriter
    {
        private Stream _stream;
        private long _remaining;
        private int _remainingPadding;
        private byte[] _buffer = new byte[3 * TarUtils.BlockSize];
        private byte[] _paxBuffer = new byte[2 * TarUtils.BlockSize];
        private TarWriterStream _currentStream;

        static private UTF8Encoding _utf8 = new UTF8Encoding(false);

        public TarWriter(Stream stream)
        {
            _stream = stream;
            _currentStream = new TarWriterStream(this);
        }

/*
struct header_old_tar {
	char name[100];
	char mode[8];
	char uid[8];
	char gid[8];
	char size[12];
	char mtime[12];
	char checksum[8];
	char linkflag[1];
	char linkname[100];
	char pad[255];
};

struct header_posix_ustar {
	char name[100];
	char mode[8];
	char uid[8];
	char gid[8];
	char size[12];
	char mtime[12];
	char checksum[8];
	char typeflag[1];
	char linkname[100];
	char magic[6];
	char version[2];
	char uname[32];
	char gname[32];
	char devmajor[8];
	char devminor[8];
	char prefix[155];
	char pad[12];
};
        */

        public Stream CurrentFile
        {
            get { return _currentStream; }
        }

        private struct WriteState
        {
            public byte[] Buffer;
            public int Offset;
        }

        bool TryPutUTF8(ref WriteState state, byte[] bytes, int offset, int count, int n)
        {
            if (count >= n)
            {
                PutNul(ref state, n);
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                state.Buffer[state.Offset + i] = bytes[offset + i];
            }

            state.Offset += count;
            PutNul(ref state, n - count);
            return true;
        }

        bool TryPutString(ref WriteState state, string str, int n)
        {
            if (str == null)
            {
                PutNul(ref state, n);
                return true;
            }

            var count = _utf8.GetByteCount(str);
            if (count >= n)
            {
                PutNul(ref state, n);
                return false;
            }

            _utf8.GetBytes(str, 0, str.Length, state.Buffer, state.Offset);
            state.Offset += count;
            PutNul(ref state, n - count);
            return true;
        }

        bool TryPutOctal(ref WriteState state, long value, int n)
        {
            if (value < 0)
            {
                PutNul(ref state, n);
                return false;
            }

            var str = Convert.ToString(value, 8);
            return TryPutString(ref state, str, n);
        }

        bool TryPutTime(ref WriteState state, DateTime time, int n)
        {
            uint nanoseconds;
            long unixTime = TarTime.ToUnixTime(time, out nanoseconds);
            if (!TryPutOctal(ref state, unixTime, n))
            {
                return false;
            }

            return nanoseconds == 0;
        }

        void PutNul(ref WriteState state, int n)
        {
            for (int i = 0; i < n; i++)
            {
                state.Buffer[state.Offset + i] = 0;
            }

            state.Offset += n;
        }

        bool TrySplitPath(string path, out byte[] bytes, out int splitIndex)
        {
            bytes = _utf8.GetBytes(path);
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == (byte)'/')
                {
                    if (i < 155 && path.Length - i - 1 < 100)
                    {
                        splitIndex = i;
                        return true;
                    }
                }
            }

            bytes = null;
            splitIndex = -1;
            return false;
        }

        public async Task AddEntryAsync(TarEntry entry)
        {
            ValidateWroteAll();

            var paxHeaders = new Dictionary<string, string>();

            var state = new WriteState
            {
                Buffer = _buffer
            };

            PutNul(ref state, _remainingPadding);
            var padding = _remainingPadding;
            _remainingPadding = 0;

            var nameState = state;
            if (!TryPutString(ref state, entry.Name, 100))
            {
                paxHeaders["path"] = entry.Name;
            }

            TryPutOctal(ref state, entry.Mode, 8);
            if (!TryPutOctal(ref state, entry.UserID, 8))
            {
                paxHeaders["uid"] = entry.UserID.ToString();
            }

            if (!TryPutOctal(ref state, entry.GroupID, 8))
            {
                paxHeaders["gid"] = entry.GroupID.ToString();
            }

            if (!TryPutOctal(ref state, entry.Length, 12))
            {
                paxHeaders["size"] = entry.Length.ToString();
            }

            if (!TryPutTime(ref state, entry.ModifiedTime, 12))
            {
                paxHeaders["mtime"] = TarTime.ToPaxTime(entry.ModifiedTime);
            }

            var checksumState = state;
            PutNul(ref state, 7);
            state.Buffer[state.Offset] = (byte)' ';
            state.Offset++;

            state.Buffer[state.Offset] = (byte)entry.Type;
            state.Offset++;

            if (!TryPutString(ref state, entry.LinkTarget, 100))
            {
                paxHeaders["linkpath"] = entry.LinkTarget;
            }

            TryPutString(ref state, "ustar", 6);

            state.Buffer[state.Offset] = (byte)'0';
            state.Buffer[state.Offset + 1] = (byte)'0';
            state.Offset += 2;

            if (!TryPutString(ref state, entry.UserName, 32))
            {
                paxHeaders["uname"] = entry.UserName;
            }

            if (!TryPutString(ref state, entry.GroupName, 32))
            {
                paxHeaders["gname"] = entry.GroupName;
            }

            if (!TryPutOctal(ref state, entry.DeviceMajor, 8))
            {
                paxHeaders["SCHILY.devmajor"] = entry.DeviceMajor.ToString();
            }

            if (!TryPutOctal(ref state, entry.DeviceMinor, 8))
            {
                paxHeaders["SCHILY.devminor"] = entry.DeviceMinor.ToString();
            }

            if (entry.AccessTime.HasValue)
            {
                paxHeaders["atime"] = TarTime.ToPaxTime(entry.AccessTime.Value);
            }

            if (entry.ChangeTime.HasValue)
            {
                paxHeaders["ctime"] = TarTime.ToPaxTime(entry.ChangeTime.Value);
            }

            if (paxHeaders.Count == 1 && paxHeaders.ContainsKey("path"))
            {
                byte[] bytes;
                int splitIndex;
                if (TrySplitPath(entry.Name, out bytes, out splitIndex))
                {
                    Console.WriteLine("splitting {0} at {1}", entry.Name, splitIndex);
                    TryPutUTF8(ref state, bytes, 0, splitIndex, 155);
                    TryPutUTF8(ref nameState, bytes, splitIndex + 1, bytes.Length - splitIndex - 1, 100);
                    paxHeaders.Clear();
                }
            }

            PutNul(ref state, padding + TarUtils.BlockSize - state.Offset);

            int signedChecksum;
            var checksum = TarUtils.Checksum(_buffer, padding, out signedChecksum);
            TryPutOctal(ref checksumState, checksum, 7);

            if (paxHeaders.Count > 0)
            {
                throw new NotSupportedException();
            }

            await _stream.WriteAsync(_buffer, 0, state.Offset);
            _remaining = entry.Length;
            _remainingPadding = TarUtils.BlockSize - (int)(entry.Length % TarUtils.BlockSize);
            if (_remainingPadding == TarUtils.BlockSize)
            {
                _remainingPadding = 0;
            }
        }

        private void ZeroArray(byte[] buffer, int offset, int length)
        {
            for (int i = 0; i < length; i++)
            {
                buffer[offset + i] = 0;
            }
        }

        public async Task CloseAsync()
        {
            ValidateWroteAll();
            ZeroArray(_buffer, 0, _buffer.Length);
            await _stream.WriteAsync(_buffer, 0, _remainingPadding + TarUtils.BlockSize * 2);
        }

        private void ValidateWroteAll()
        {
            if (_remaining > 0)
            {
                throw new Exception(string.Format("did not finish writing last entry: {0}", _remaining));
            }
        }

        internal void Write(byte[] buffer, int offset, int count)
        {
            if (count > _remaining)
            {
                throw new Exception("wrote too much");
            }

            _stream.Write(buffer, offset, count);
            _remaining -= count;
        }

        internal async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (count > _remaining)
            {
                throw new Exception("wrote too much");
            }

            await _stream.WriteAsync(buffer, offset, count, cancellationToken);
            _remaining -= count;
        }
    }
}