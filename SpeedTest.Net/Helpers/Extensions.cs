using SpeedTest.Net.Enums;

namespace SpeedTest.Net.Helpers
{
    internal static class Extensions
    {
        internal static string ToSourceString(this SpeedTestSource source)
        {
            if (source == SpeedTestSource.Fast)
                return "fast.com";

            return "speedtest.net";
        }

        internal static string ToShortIdentifier(this SpeedTestUnit unit)
        {
            if (unit == SpeedTestUnit.MegaBytesPerSecond)
                return "MB/s";

            if (unit == SpeedTestUnit.MegaBitsPerSecond)
                return "Mb/s";

            if (unit == SpeedTestUnit.KiloBitsPerSecond)
                return "Kb/s";

            return "KB/s";
        }

        internal static double FromBytesPerSecondTo(this double speed, SpeedTestUnit unit)
        {
            switch (unit)
            {
                case SpeedTestUnit.BytesPerSecond:
                    return speed;
                case SpeedTestUnit.KiloBytesPerSecond:
                    return speed / 1024;
                case SpeedTestUnit.KiloBitsPerSecond:
                    return speed * 8 / 1024;
                case SpeedTestUnit.MegaBytesPerSecond:
                    return speed / (1024 * 1024);
                case SpeedTestUnit.MegaBitsPerSecond:
                    return speed * 8 / (1024 * 1024);
                default:
                    return speed;
            }
        }
    }
}