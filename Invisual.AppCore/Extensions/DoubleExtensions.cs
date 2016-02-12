using System;

namespace Invisual.AppCore.Extensions
{
    public static class DoubleExtensions
    {
        public static DateTime FromUnixTimeStamp(double unixTimeStamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTimeStamp).ToLocalTime();
        }
    }
}
