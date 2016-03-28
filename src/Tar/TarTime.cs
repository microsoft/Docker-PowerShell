using System;

namespace Tar
{
    class TarTime
    {
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
        public static DateTime FromPaxTime(string str)
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

            return FromUnixTime(seconds, nanoseconds);
        }

        public const int TicksPerSecond = 10 * 1000 * 1000;
        public const int NanoSecondsPerTick = 100;
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly long MinUnixTime = ToUnixTime(DateTime.MinValue);
        public static readonly long MaxUnixTime = ToUnixTime(DateTime.MaxValue);
    }
}
