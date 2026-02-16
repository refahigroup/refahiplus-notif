using System.Globalization;

namespace Refahi.Notif.Domain.Core.Utility
{
    public static class ConvertTime
    {
        public static string ToSitemapDatetime(this DateTime date)
        {
            CultureInfo cultureinfo =
        new CultureInfo("en-US");

            return date.ToString("yyyy-MM-dd", cultureinfo);
            return $"{new DateTimeOffset(date).ToString("o")}";
        }
        public static DateTime ToMiladi(this string dateTime)
        {
            var date = dateTime.Fa2En().Split('-')[0];

            var year = int.Parse(date.Split('/')[0]);
            var month = int.Parse(date.Split('/')[1]);
            var day = int.Parse(date.Split('/')[2]);
            int hour = 0, minute = 0;
            try
            {
                var time = dateTime.Fa2En().Split('-')[1];

                hour = int.Parse(time.Split(':')[0]);
                minute = int.Parse(time.Split(':')[1]);
            }
            catch { }

            return new DateTime(year, month, day, hour, minute, 0, new PersianCalendar());
        }
        public static string ToPersian(this DateTime? time)
        {
            if (time == null)
                return "";
            else
            {
                return time.Value.ToPersian();
            }
        }
        public static string ToPersian(this DateTime time2, bool withTime = false)
        {
            return ((DateTime?)time2).ToPersian(withTime);
        }
        public static string ToPersian(this DateTime? time2, bool withTime = false)
        {
            try
            {
                if (time2 == null)
                    return "";
                var time = time2.Value;

                var hour = new PersianCalendar().GetHour(time).ToString();

                if (int.Parse(hour) > 12)
                    hour = (int.Parse(hour) - 12).ToString();
                if (hour.Length == 1)
                    hour = "0" + hour;

                var minute = new PersianCalendar().GetMinute(time).ToString();
                if (minute.Length == 1)
                    minute = "0" + minute;



                var ampm = time.ToString("tt", CultureInfo.InvariantCulture);
                if (ampm.ToLower() == "pm" && hour != "12")
                    hour = (int.Parse(hour) + 12).ToString();

                var year = new PersianCalendar().GetYear(time);
                var month = new PersianCalendar().GetMonth(time).TwoCharacter();
                var day = new PersianCalendar().GetDayOfMonth(time).TwoCharacter();


                var result =
                     $"{year}/{month}/{day}"
                       ;
                if (!withTime)
                {
                    return result;
                }
                else
                    return $"{result}-{hour}:{minute}";
                //+  $"{ampm}"

                ;

            }
            catch { }
            return null;
        }
        public static string ToUserFreindlyTime(this DateTime time)
        {

            var now = DateTime.Now;
            var dif = now - time;
            if (dif.TotalDays > 365)
                return $"{NotSign(dif.TotalDays / 365)} سال پیش";
            if (dif.TotalDays > 30)
                return $"{NotSign(dif.TotalDays / 30)} ماه پیش";
            if (dif.TotalDays > 14)
                return $"{NotSign(dif.TotalDays / 7)} هفته پیش";
            if (dif.TotalDays > 2)
                return $"{NotSign(dif.TotalDays / 1)} روز پیش";
            if (dif.TotalDays > 1)
                return $"دیروز";
            if (dif.TotalHours > 1)
                return $"{NotSign(dif.TotalHours)} ساعت پیش";
            if (dif.TotalMinutes > 1)
                return $"{NotSign(dif.TotalMinutes)} دقیقه پیش";
            return $"الان";
        }
        public static string ToUserFreindlyTimeOfWeek(this DateTime time)
        {

            var now = DateTime.Now.Date;
            var dif = now - time.Date;

            if (dif.TotalDays > 365)
                return $"{NotSign(dif.TotalDays / 365)} سال پیش";
            if (dif.TotalDays > 30)
                return $"{NotSign(dif.TotalDays / 30)} ماه پیش";
            if (dif.TotalDays > 14)
                return $"{NotSign(dif.TotalDays / 7)} هفته پیش";

            if (dif.TotalDays >= 2)
                return PersianDayOfWeek(time.DayOfWeek);
            if (dif.TotalDays == 1)
                return $"دیروز";
            if (dif.TotalDays == 0)
                return $"امروز";
            if (dif.TotalDays == -1)
                return $"فردا";
            if (dif.TotalDays > -5)
                return PersianDayOfWeek(time.DayOfWeek);
            return time.ToPersian();
        }
        private static string PersianDayOfWeek(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Saturday:
                    return "شنبه";
                case DayOfWeek.Sunday:
                    return "یکشنبه";
                case DayOfWeek.Monday:
                    return "دوشنبه";
                case DayOfWeek.Tuesday:
                    return "سه شنبه";
                case DayOfWeek.Wednesday:
                    return "چهارشنبه";
                case DayOfWeek.Thursday:
                    return "پنج شنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
            }
            return "";
        }
        private static string NotSign(double str)
        {
            return str.ToString().Split('.')[0].Split('/')[0];
        }

        public static long ToUnixSeconds(this DateTime dateTime)
        {
            return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        }
    }

}
