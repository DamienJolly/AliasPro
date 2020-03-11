using System;

namespace AliasPro.Utilities
{
    public static class UnixTimestamp
    {
        public static DateTime FromUnixTimestamp(double timestamp)
        {
            DateTime DT = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DT = DT.AddSeconds(timestamp);
            return DT;
        }

        public static double Now => (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

        public static bool IsToday(double timestamp) =>
            IsToday(FromUnixTimestamp(timestamp));

        public static bool IsToday(DateTime time) =>
            time.Date == DateTime.Today;

        public static bool IsYesterday(double timestamp) =>
            IsYesterday(FromUnixTimestamp(timestamp));

        public static bool IsYesterday(DateTime time) =>
            DateTime.Today - time.Date == TimeSpan.FromDays(1);
    }
}
