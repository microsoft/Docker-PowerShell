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

        public const int TicksPerSecond = 10 * 1000 * 1000;
        public const int NanoSecondsPerTick = 100;
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly long MinUnixTime = ToUnixTime(DateTime.MinValue);
        public static readonly long MaxUnixTime = ToUnixTime(DateTime.MaxValue);
    }
}
