using System;

namespace Tar
{
    internal static class TarUtils
    {
        public const string PosixMagic = "ustar";
        public const string GnuMagic = "ustar  ";

        public const TarEntryType PaxHeaderType = (TarEntryType)'x';
        public const TarEntryType PaxGlobalHeaderType = (TarEntryType)'g';

        public const string PaxUid = "uid";
        public const string PaxGid = "gid";
        public const string PaxUname = "uname";
        public const string PaxGname = "gname";
        public const string PaxSize = "size";
        public const string PaxLinkpath = "linkpath";
        public const string PaxPath = "path";
        public const string PaxCtime = "ctime";
        public const string PaxAtime = "atime";
        public const string PaxMtime = "mtime";
        public const string PaxDevmajor = "SCHILY.devmajor";
        public const string PaxDevminor = "SCHILY.devminor";

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
    }
}
