using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tar
{
    public class TarWriter
    {
        private readonly Stream _stream;
        private long _remaining;
        private int _remainingPadding;
        private readonly byte[] _buffer = new byte[3 * TarCommon.BlockSize];
        private readonly TarWriterStream _currentEntryStream;

        public TarWriter(Stream stream)
        {
            _stream = stream;
            _currentEntryStream = new TarWriterStream(this);
        }

        public Stream CurrentFile => _currentEntryStream;

        // Tries to split a path at a '/' character so that the two pieces will fit into
        // the prefix and name fields.
        private static bool TrySplitPath(string path, out int splitIndex)
        {
            splitIndex = -1;
            if (!TarCommon.IsASCII(path))
            {
                return false;
            }

            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '/')
                {
                    if (i < TarHeader.Prefix.Length && path.Length - i - 1 < TarHeader.Name.Length)
                    {
                        splitIndex = i;
                        return true;
                    }
                }
            }

            return false;
        }

        private static void GenerateHeader(ref TarHeaderView header, TarEntry entry, ArraySegment<byte> name)
        {
            var needsPath = false;

            header.PutNul(TarHeader.FullHeader);

            // Try to write the name, but don't write the PAX attribute yet in case we can
            // just write a split name later.
            if (name.Count > 0)
            {
                header.PutBytes(name, TarHeader.Name);
            }
            else if (!header.TryPutString(entry.Name, TarHeader.Name.WithoutPax))
            {
                needsPath = true;
            }

            header.TryPutOctal(entry.Mode, TarHeader.Mode);
            header.TryPutOctal(entry.UserID, TarHeader.UserID);
            header.TryPutOctal(entry.GroupID, TarHeader.GroupID);
            header.TryPutOctal(entry.Length, TarHeader.Length);
            header.TryPutTime(entry.ModifiedTime, TarHeader.ModifiedTime);

            header[TarHeader.ChecksumSpace.Offset] = (byte)' ';

            header[TarHeader.TypeFlag.Offset] = (byte)entry.Type;

            header.TryPutString(entry.LinkTarget, TarHeader.LinkTarget);

            header.TryPutString(TarCommon.PosixMagic, TarHeader.Magic);
            header[TarHeader.Version.Offset] = (byte)'0';
            header[TarHeader.Version.Offset + 1] = (byte)'0';

            header.TryPutString(entry.UserName, TarHeader.UserName);
            header.TryPutString(entry.GroupName, TarHeader.GroupName);
            header.TryPutOctal(entry.DeviceMajor, TarHeader.DeviceMajor);
            header.TryPutOctal(entry.DeviceMinor, TarHeader.DeviceMinor);

            if (header.PaxAttributes != null)
            {
                if (entry.AccessTime.HasValue)
                {
                    header.PaxAttributes[TarCommon.PaxAtime] = TarTime.ToPaxTime(entry.AccessTime.Value);
                }

                if (entry.ChangeTime.HasValue)
                {
                    header.PaxAttributes[TarCommon.PaxCtime] = TarTime.ToPaxTime(entry.ChangeTime.Value);
                }

                if (entry.CreationTime.HasValue)
                {
                    header.PaxAttributes[TarCommon.PaxCreationTime] = TarTime.ToPaxTime(entry.CreationTime.Value);
                }

                if (entry.FileAttributes.HasValue)
                {
                    header.PaxAttributes[TarCommon.PaxWindowsFileAttributes] = Convert.ToString((uint)entry.FileAttributes.Value);
                }

                if (entry.IsMountPoint)
                {
                    header.PaxAttributes[TarCommon.PaxWindowsMountPoint] = "1";
                }

                if (entry.SecurityDescriptor != null)
                {
                    header.PaxAttributes[TarCommon.PaxWindowsSecurityDescriptor] = entry.SecurityDescriptor;
                }

                if (needsPath)
                {
                    int splitIndex;
                    if (header.PaxAttributes.Count == 0 && TrySplitPath(entry.Name, out splitIndex))
                    {
                        header.TryPutString(entry.Name.Substring(0, splitIndex), TarHeader.Prefix);
                        header.TryPutString(entry.Name.Substring(splitIndex + 1), TarHeader.Name);
                    }
                    else
                    {
                        header.PaxAttributes[TarHeader.Name.PaxAttribute] = entry.Name;
                    }
                }
            }

            int signedChecksum;
            var checksum = TarCommon.Checksum(header.Field(TarHeader.FullHeader), out signedChecksum);
            header.TryPutOctal(checksum, TarHeader.Checksum);
        }

        private static void WritePaxAttribute(Stream stream, string key, string value)
        {
            var entryText = TarCommon.UTF8.GetBytes(string.Format(" {0}={1}\n", key, value));
            var entryLength = entryText.Length;
            entryLength += entryLength.ToString().Length;
            var entryLengthString = entryLength.ToString();
            if (entryLengthString.Length + entryText.Length != entryLength)
            {
                entryLength = entryLengthString.Length + entryText.Length;
                entryLengthString = entryLength.ToString();
            }

            var entryLengthString8 = TarCommon.UTF8.GetBytes(entryLengthString);

            stream.Write(entryLengthString8, 0, entryLengthString8.Length);
            stream.Write(entryText, 0, entryText.Length);
        }

        // Create a name to use for the PAX entry for tar implementations that do not support PAX.
        private ArraySegment<byte> GetPaxName(string name)
        {
            var path = name.TrimEnd('/', '\\');
            var dirName = Path.GetDirectoryName(path).Replace('\\', '/');
            var fileName = Path.GetFileName(path);
            if (dirName == "")
            {
                dirName = ".";
            }

            return new ArraySegment<byte>(TarCommon.UTF8.GetBytes(string.Format("{0}/PaxHeaders.0/{1}", dirName, fileName)));
        }

        private async Task<int> WritePaxEntry(TarEntry entry, Dictionary<string, string> paxAttributes, int padding)
        {
            var paxStream = new MemoryStream();

            // Skip the tar header, then go back and write it later.
            var paxDataOffset = padding + TarCommon.BlockSize;
            paxStream.SetLength(paxDataOffset);
            paxStream.Position = paxDataOffset;

            foreach (var attribute in paxAttributes)
            {
                WritePaxAttribute(paxStream, attribute.Key, attribute.Value);
            }

            var paxEntry = new TarEntry
            {
                Type = TarCommon.PaxHeaderType,
                Length = paxStream.Length - paxDataOffset,
            };

            ArraySegment<byte> buffer;
            paxStream.TryGetBuffer(out buffer);

            var paxHeader = new TarHeaderView(buffer.Array, buffer.Offset + padding, null);
            GenerateHeader(ref paxHeader, paxEntry, GetPaxName(entry.Name));

            paxStream.Position = 0;
            await paxStream.CopyToAsync(_stream);
            return TarCommon.Padding(paxEntry.Length);
        }

        public async Task CreateEntryAsync(TarEntry entry)
        {
            ValidateWroteAll();

            var paxAttributes = new Dictionary<string, string>();

            // Clear padding.
            for (int i = 0; i < TarCommon.BlockSize; i++)
            {
                _buffer[i] = 0;
            }

            var state = new TarHeaderView(_buffer, TarCommon.BlockSize, paxAttributes);
            var padding = _remainingPadding;
            _remainingPadding = 0;

            GenerateHeader(ref state, entry, default(ArraySegment<byte>));

            if (paxAttributes.Count > 0)
            {
                padding = await WritePaxEntry(entry, paxAttributes, padding);
            }

            await _stream.WriteAsync(_buffer, TarCommon.BlockSize - padding, TarCommon.BlockSize + padding);
            _remaining = entry.Length;
            _remainingPadding = TarCommon.Padding(_remaining);
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
            await _stream.WriteAsync(_buffer, 0, _remainingPadding + TarCommon.BlockSize * 2);
        }

        private void ValidateWroteAll()
        {
            if (_remaining > 0)
            {
                throw new InvalidOperationException("caller did not finish writing last entry");
            }
        }

        internal void WriteCurrentFile(byte[] buffer, int offset, int count)
        {
            if (count > _remaining)
            {
                throw new InvalidOperationException("caller wrote more than the specified number of bytes");
            }

            _stream.Write(buffer, offset, count);
            _remaining -= count;
        }

        internal async Task WriteCurrentFileAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (count > _remaining)
            {
                throw new InvalidOperationException("caller wrote more than the specified number of bytes");
            }

            await _stream.WriteAsync(buffer, offset, count, cancellationToken);
            _remaining -= count;
        }
    }
}