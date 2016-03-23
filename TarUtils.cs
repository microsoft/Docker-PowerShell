namespace Tar
{
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
}
