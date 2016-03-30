using System;
using System.Text;

namespace Tar
{
    internal static class TarCommon
    {
        public const string PosixMagic = "ustar";
        public const string GnuMagic = "ustar  ";

        public const TarEntryType PaxHeaderType = (TarEntryType)'x';
        public const TarEntryType PaxGlobalHeaderType = (TarEntryType)'g';
        public const TarEntryType GnuLongLinknameType = (TarEntryType)'K';
        public const TarEntryType GnuLongPathnameType = (TarEntryType)'L';

        public const string PaxCtime = "ctime";
        public const string PaxAtime = "atime";
        public const string PaxCreationTime = "LIBARCHIVE.creationtime";
        public const string PaxWindowsSecurityDescriptor = "MSWINDOWS.sd";
        public const string PaxWindowsFileAttributes = "MSWINDOWS.fileattr";
        public const string PaxWindowsMountPoint = "MSWINDOWS.mountpoint";

        public const int BlockSize = 512;

        public static readonly ASCIIEncoding ASCII = new ASCIIEncoding();
        public static readonly UTF8Encoding UTF8 = new UTF8Encoding(false);

        public static int Checksum(ArraySegment<byte> buffer, out int signedChecksum)
        {
            signedChecksum = 0;
            int unsignedChecksum = 0;
            for (int i = 0; i < BlockSize; i++)
            {
                byte b = buffer.Array[buffer.Offset + i];

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

        public static int Padding(long fileSize)
        {
            int padding = BlockSize - (int)(fileSize % BlockSize);
            if (padding == BlockSize)
            {
                padding = 0;
            }

            return padding;
        }

        public static bool IsASCII(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] > 127)
                {
                    return false;
                }
            }

            return true;
        }

        public static string MakeASCII(string s)
        {
            var sb = new StringBuilder();
            for (int i =0 ; i < s.Length; i++)
            {
                sb.Append(s[i] <= 127 ? s[i] : '?');
            }
            return sb.ToString();
        }
    }
}
