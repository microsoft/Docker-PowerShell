using System;

namespace Tar
{
    internal static class TarUtils
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
    }
}
