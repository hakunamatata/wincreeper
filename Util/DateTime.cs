using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public static class DateTimeExtention
    {
        /// <summary>
        /// 时间转为时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTime date)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (date.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位
        }

        public static int ToTimeStamp32(this DateTime date)
        {
            return Convert.ToInt32(ToTimeStamp(date) / 10000);
        }

        public static DateTime ToDateTime(this string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        public static bool IsTimeStamp(this long timestamp, double interval)
        {
            DateTime dt = GetDateTimeFrom1970Ticks(timestamp);
            //取现在时间
            DateTime dt1 = DateTime.Now.AddMinutes(interval);
            DateTime dt2 = DateTime.Now.AddMinutes(interval * -1);
            return dt > dt2 && dt < dt1;
        }

        private static DateTime GetDateTimeFrom1970Ticks(long curSeconds)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(curSeconds);
        }

        private static DateTime GetDateTimeFrom1970Ticks(int curSeconds)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(curSeconds);
        }

        public static DateTime ToDateTime(this int curSeconds)
        {
            return GetDateTimeFrom1970Ticks(curSeconds);
        }
    }
}
