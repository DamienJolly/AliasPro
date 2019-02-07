using System;

namespace AliasPro.Utilities
{
    public static class UnixTimestamp
    {
        public static DateTime FromUnixTimestamp(double Timestamp)
        {
            DateTime DT = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DT = DT.AddSeconds(Timestamp);
            return DT;
        }

        public static double Now => (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
    }
}
