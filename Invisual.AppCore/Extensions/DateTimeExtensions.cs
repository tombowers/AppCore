using System;

namespace Invisual.AppCore.Extensions
{
    public static class DateTimeExtensions
    {
        public static double ToUnixTimeStamp(this DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (dateTime - epoch.ToLocalTime()).TotalSeconds;
        }
    }
}
