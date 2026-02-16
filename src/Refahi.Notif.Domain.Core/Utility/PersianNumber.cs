namespace Refahi.Notif.Domain.Core.Utility
{
    public static class PersianNumber
    {
        //تبدیل اعداد انگلیسی به فارسی
        public static string En2Fa(this string str)
        {
            return str.Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴").Replace("5", "۵").Replace("6", "۶").Replace("7", "۷").Replace("8", "۸").Replace("9", "۹");
        }
        //تبدیل اعداد فارسی به انگلیسی
        public static string Fa2En(this string str)
        {
            return str?.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");
        }
    }
}
