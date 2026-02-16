using System.Text;
using System.Text.RegularExpressions;

namespace Refahi.Notif.Domain.Core.Utility
{
    public static class StringTools
    {
        public static string ToEncodedString(this Stream stream, Encoding enc = null)
        {
            try
            {
                enc = enc ?? Encoding.UTF8;

                byte[] bytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(bytes, 0, (int)stream.Length);
                string data = enc.GetString(bytes);

                return enc.GetString(bytes);

                //using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                //{
                //    return reader.ReadToEndAsync().Result;
                //}
            }
            catch { return null; }
        }
        public static string Join(this string[] array, string charachter = ",")
        {
            return string.Join(charachter, array);
        }
        public static string ToUnknown(this string temp)
        {
            return $"{temp.Substring(0, 3)}***{temp.Substring(6, 4)}";
        }

        public static string TwoCharacter(this int number)
        {
            if (number > 9)
                return number.ToString();
            return $"0{number}";
        }

        //تبدیل هر چیزی به نوع عددی int
        public static int ToInt(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj.ToString().Replace(",", "").Fa2En());
            }
            catch
            {
                return Convert.ToInt32(obj);
            }
        }

        public static string ToUrl(this string value)
        {
            //First to lower case 
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("UTF-8").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces 
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;


        }

        public static string RemoveHtml(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            return Regex.Replace(input, "<.*?>", string.Empty).Replace("\u200c", " ").Replace("&nbsp;", " ");
        }
        public static string FirstChars(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.Length > length + 3)
                return str.Remove(length + 3) + "...";
            return str;
        }

        private static readonly RegexOptions regexOptions = RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase;

        private static readonly string phoneNumberPattern = @"^(\+?98|098|0|0098)?(9\d{9})$";

        private static readonly Regex PhoneNumberRegex = new Regex(phoneNumberPattern, regexOptions | RegexOptions.RightToLeft, TimeSpan.FromMilliseconds(100));


        public static bool IsValidMobilePhoneNumber(this string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber) && PhoneNumberRegex.IsMatch(phoneNumber);
        }

        public static string NormalizePhoneNumber(this string phoneNumber)
        {
            if (!phoneNumber.IsValidMobilePhoneNumber())
                return string.Empty;

            var match = PhoneNumberRegex.Match(phoneNumber);

            if (match.Success && match.Groups.TryGetValue("2", out var value))
                return value.Value;

            return string.Empty;
        }

        public static string NormalizePhoneNumberWithCountryCode(this string phoneNumber)
        {
            return $"98{phoneNumber.NormalizePhoneNumber()}";
        }
    }
}
