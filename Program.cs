using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var file = File.OpenRead(args[0]);
            var file2 = File.Create(args[1]);
            var reader = new TarReader(file);
            var writer = new TarWriter(file2);
            for (;;)
            {
                var entryTask = reader.GetNextEntryAsync();
                entryTask.Wait();
                var entry = entryTask.Result;
                if (entry == null)
                {
                    break;
                }
                                
                var task = writer.AddEntryAsync(entry);
                try 
                {
                    task.Wait();
                } 
                catch (Exception e)
                {
                    throw e;
                }
                
                var buffer = new byte[entry.Length];
                if (reader.CurrentFile.Read(buffer, 0, buffer.Length) != buffer.Length)
                {
                    throw new Exception("what");
                }
                
                writer.CurrentFile.Write(buffer, 0, buffer.Length);
                reader.CurrentFile.CopyTo(writer.CurrentFile);
            }
            
            writer.CloseAsync().Wait();
        }
    }
    
    internal class TarUtils
    {
        public const TarEntryType PaxHeaderType = (TarEntryType)'x';
        public const TarEntryType PaxGlobalHeaderType = (TarEntryType)'g';
        
        public const int BlockSize = 512;           

        public static int Checksum(byte[] buffer, int offset, out int signedChecksum)
        {
            signedChecksum = 0;
            int unsignedChecksum = 0;
            for (int i = 0; i < TarUtils.BlockSize; i++)
            {
                byte b = buffer[offset + i];
                
                // The checksum portion of the header is cleared with space characters. 
                if (i >= 148 && i < 156)
                {
                    b = (byte)' ';
                }
                
                // These are guaranteed not to wrap because the header is so small.
                signedChecksum += (sbyte)b;
                unsignedChecksum += b;
            }

            return unsignedChecksum;
        }        

        public static long ToUnixTime(DateTime time, out uint nanoseconds)
        {
            var ticks = (time - UnixEpoch).Ticks;
            nanoseconds = (uint)(ticks % TicksPerSecond * 100);
            return ticks / TicksPerSecond;
        }
        
        public static long ToUnixTime(DateTime time)
        {
            uint nanoseconds;
            return ToUnixTime(time, out nanoseconds);
        }
        
        public static DateTime FromUnixTime(long time, uint nanoseconds)
        {
            int snano = (int)nanoseconds;
            if (time < 0)
            {
                snano = -snano;
            }
            
            return new DateTime(UnixEpoch.Ticks + time * TicksPerSecond + snano / NanoSecondsPerTick, DateTimeKind.Utc);
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

            return FromUnixTime(seconds, nanoseconds);
        }
        
        public static string ToPaxTime(DateTime time)
        {
            uint nanoseconds;
            long seconds = ToUnixTime(time, out nanoseconds);
            if (nanoseconds != 0)
            {
                return string.Format("{0}.{1:D7}", seconds, nanoseconds / 100);
            }
            else
            {
                return Convert.ToString(seconds);
            }
        }
        
        public const int TicksPerSecond = 10 * 1000 * 1000;
        public const int NanoSecondsPerTick = 100;
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly long MinUnixTime = ToUnixTime(DateTime.MinValue);
        public static readonly long MaxUnixTime = ToUnixTime(DateTime.MaxValue);   
    }
    
    internal class TarReaderStream : Stream
    {        
        private TarReader _reader;
        
        public TarReaderStream(TarReader reader)
        {
            _reader = reader;
        }
        
        public override bool CanRead 
        {
            get { return true; }
        }
        
        public override bool CanSeek
        {
            get { return false; }
        }
        
        public override bool CanTimeout
        {
            get { return false; /* should return underlying stream */ }
        }
        
        public override bool CanWrite
        {
            get { return false; }
        }
        
        public override long Length
        {
            get { throw new NotSupportedException(); }
        }
        
        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }
        
        public override void Flush()
        {
        }
        
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _reader.Read(buffer, offset, count);
        }
        
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _reader.ReadAsync(buffer, offset, count, cancellationToken);
        }
        
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }
        
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
    
    internal class TarWriterStream : Stream
    {   
        private TarWriter _writer;
        
        public TarWriterStream(TarWriter writer)
        {
            _writer = writer;
        }
        
        public override bool CanRead 
        {
            get { return false; }
        }
        
        public override bool CanSeek
        {
            get { return false; }
        }
        
        public override bool CanTimeout
        {
            get { return false; /* should return underlying stream */ }
        }
        
        public override bool CanWrite
        {
            get { return true; }
        }
        
        public override long Length
        {
            get { throw new NotSupportedException(); }
        }
        
        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }
        
        public override void Flush()
        {
        }
        
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
        
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }
        
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        
        public override void Write(byte[] buffer, int offset, int count)
        {
            _writer.Write(buffer, offset, count);            
        }
        
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _writer.WriteAsync(buffer, offset, count, cancellationToken);
        } 
    }

    public enum TarEntryType : byte
    {
        File = (byte)'0',
        HardLink,
        SymbolicLink,
        CharacterDevice,
        BlockDevice,
        Directory,
        FIFO,
    }
    
    public class TarEntry
    {
        private string _name;
        private long _mode;
        private int _userID;
        private int _groupID;
        private long _length;
        private DateTime _ModifiedTime;
        private DateTime? _accessTime;
        private DateTime? _changeTime;
        private TarEntryType _type = TarEntryType.File;
        private string _linkTarget;
        private string _userName;
        private string _groupName;
        private long _deviceMajor;
        private long _deviceMinor;
        
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public long Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }
        
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        
        public int GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }
        
        public long Length
        {
            get { return _length; }
            set { _length = value; }
        }
        
        public DateTime ModifiedTime
        {
            get { return _ModifiedTime; }
            set { _ModifiedTime = value; }
        }
        
        public DateTime? AccessTime
        {
            get { return _accessTime; }
            set { _accessTime = value; }
        }

        public DateTime? ChangeTime
        {
            get { return _changeTime; }
            set { _changeTime = value; }
        }
        
        public TarEntryType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string LinkTarget
        {
            get { return _linkTarget; }
            set { _linkTarget = value; }
        }
        
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }
        
        public long DeviceMajor
        {
            get { return _deviceMajor; }
            set { _deviceMajor = value; }
        }
        
        public long DeviceMinor
        {
            get { return _deviceMinor; }
            set { _deviceMinor = value; }
        }        
    }
    
    public class TarReader
    {
        internal Stream _stream;
        internal TarReaderStream _currentStream;
        private byte[] _buffer = new byte[2 * TarUtils.BlockSize];
        private long _remaining;
        private int _remainingPadding;
        static private ASCIIEncoding _ascii = new ASCIIEncoding();
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
        }
        
        private static string GetString(ref ReadState state, int length, Encoding encoding)
        {
            var s = encoding.GetString(state.Buffer, state.Offset, length);
            state.Offset += length;
            var nulIndex = s.IndexOf('\x00');
            if (nulIndex < 0)
            {
                return s;
            }
            
            return s.Substring(0, nulIndex);
        }

        static readonly char[] nulAndSpace = new char[]{' ', '\x00'};
        static readonly char[] nul = new char[]{'\x00'};

        private static long GetOctalLong(ref ReadState state, int length)
        {
            if ((state.Buffer[state.Offset] & 128) != 0)
            {
                throw new NotSupportedException();
            }
            else
            {
                var str = GetString(ref state, length, _ascii).Trim(nulAndSpace);
                if (str.Length == 0)
                {
                    return 0;
                }
                return Convert.ToInt64(str, 8);
            }
        }
        
        private static int GetOctal(ref ReadState state, int length)
        {
            return (int)GetOctalLong(ref state, length);
        }
        
        private static DateTime GetTime(ref ReadState state, int length)
        {
            long unixTime = GetOctalLong(ref state, length);
            if (unixTime < TarUtils.MinUnixTime)
            {
                unixTime = TarUtils.MinUnixTime; 
            }           
            else if (unixTime > TarUtils.MaxUnixTime)
            {
                unixTime = TarUtils.MaxUnixTime;
            }
            
            return TarUtils.FromUnixTime(unixTime, 0);
        }
        
        private static long ParsePaxNumberLong(string str)
        {
            return Convert.ToInt64(str);
        }
        
        private static int ParsePaxNumber(string str)
        {
            long value = ParsePaxNumberLong(str);
            if ((int)value != value)
            {
                throw new Exception("PAX value too large");
            }
            
            return (int)value;
        }
        
        private static TarEntry ParseHeader(byte[] buffer, int offset)
        {
            var state = new ReadState
            {
                Buffer = buffer,
                Offset = offset,
            };
            
            var entry = new TarEntry();
            
            entry.Name = GetString(ref state, 100, _utf8);
            entry.Mode = GetOctal(ref state, 8);
            entry.UserID = GetOctal(ref state, 8);
            entry.GroupID = GetOctal(ref state, 8);
            entry.Length = GetOctalLong(ref state, 12);
            entry.ModifiedTime = GetTime(ref state, 12);
            var checksum = GetOctal(ref state, 8);
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
            entry.LinkTarget = GetString(ref state, 100, _utf8);
            var magic = GetString(ref state, 8, _ascii);            
            if (magic == "ustar" || magic == "ustar  ")
            {
                // POSIX, star, or GNU format
                entry.UserName = GetString(ref state, 32, _utf8);
                entry.GroupName = GetString(ref state, 32, _utf8);
                entry.DeviceMajor = GetOctal(ref state, 8);
                entry.DeviceMinor = GetOctal(ref state, 8);
                if (magic == "ustar")
                {
                    // POSIX or star
                    var prefix = GetString(ref state, 155, _utf8);
                    if (prefix.Length > 0)
                    {
                        entry.Name = prefix + "/" + entry.Name;
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
        
        private async Task<TarEntry> ReadHeader(Dictionary<string, string> paxHeaders)
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

            var entry = ParseHeader(_buffer, padding);
            if (paxHeaders != null)
            {
                string value;
                if (paxHeaders.TryGetValue("atime", out value))
                {
                    entry.AccessTime = TarUtils.ParsePaxTime(value);
                }
                
                if (paxHeaders.TryGetValue("ctime", out value))
                {
                    entry.ChangeTime = TarUtils.ParsePaxTime(value);
                }
                
                if (paxHeaders.TryGetValue("mtime", out value))
                {
                    entry.ModifiedTime = TarUtils.ParsePaxTime(value);
                }
                
                if (paxHeaders.TryGetValue("uname", out value))
                {
                    entry.UserName = value;
                }
                
                if (paxHeaders.TryGetValue("gname", out value))
                {
                    entry.GroupName = value;
                }
                
                if (paxHeaders.TryGetValue("uid", out value))
                {
                    entry.UserID = ParsePaxNumber(value);
                }
                
                if (paxHeaders.TryGetValue("gid", out value))
                {
                    entry.GroupID = ParsePaxNumber(value);
                }
                
                if (paxHeaders.TryGetValue("linkpath", out value))
                {
                    entry.LinkTarget = value;
                }
                
                if (paxHeaders.TryGetValue("path", out value))
                {
                    entry.Name = value;
                }
                
                if (paxHeaders.TryGetValue("size", out value))
                {
                    entry.Length = ParsePaxNumberLong(value);
                }
            }

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
                var paxHeaders = await ReadPax();
                entry = await ReadHeader(paxHeaders);
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
        
        private async Task<Dictionary<string, string>> ReadPax()
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

        internal int Read(byte[] buffer, int offset, int count)
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
        
        internal async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
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
            long unixTime = TarUtils.ToUnixTime(time, out nanoseconds);
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
                paxHeaders["mtime"] = TarUtils.ToPaxTime(entry.ModifiedTime);
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
                paxHeaders["atime"] = TarUtils.ToPaxTime(entry.AccessTime.Value);
            }
            
            if (entry.ChangeTime.HasValue)
            {
                paxHeaders["ctime"] = TarUtils.ToPaxTime(entry.ChangeTime.Value);
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
