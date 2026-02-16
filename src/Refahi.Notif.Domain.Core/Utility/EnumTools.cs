using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Refahi.Notif.Domain.Core.Utility
{
    public static class EnumTools
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            try
            {
                if (enumValue == null)
                    return null;
                var item = enumValue.GetType().GetMember(enumValue.ToString())
                                .First()
                                .GetCustomAttribute<DisplayAttribute>();

                return item
                               .Name;

            }
            catch (Exception)
            {

                return enumValue.ToString();
            }
        }
        public static string GetDisplayPrompt(this Enum enumValue)
        {
            try
            {
                return enumValue.GetType().GetMember(enumValue.ToString())
                               .First()
                               .GetCustomAttribute<DisplayAttribute>()
                               .Prompt;

            }
            catch (Exception)
            {

                return string.Empty;
            }
        }
        public static string GetLabel(this Enum enumValue)
        {
            try
            {
                var prompt = enumValue.GetDisplayPrompt();
                if (string.IsNullOrEmpty(prompt))
                {
                    prompt = "secondary";
                }
                return $"<span class='badge bg-{prompt} rounded-pill'>{enumValue.GetDisplayName()}</span>";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static string GetDisplayDescription(this Enum enumValue)
        {
            string desc = "";
            try
            {
                var item = enumValue.GetType().GetMember(enumValue.ToString())
                                .First()
                                .GetCustomAttribute<DisplayAttribute>();

                desc = item
                               .Description;

            }
            catch (Exception)
            {

                desc = string.Empty;
            }
            if (string.IsNullOrEmpty(desc))
                return enumValue.GetDisplayName();
            return desc;
        }
        public static string GetDisplayGroup(this Enum enumValue)
        {
            try
            {
                return enumValue.GetType().GetMember(enumValue.ToString())
                               .First()
                               .GetCustomAttribute<DisplayAttribute>()
                               .GroupName;

            }
            catch (Exception)
            {

                return string.Empty;
            }
        }
        public static string GetDisplayShortName(this Enum enumValue)
        {
            try
            {
                return enumValue.GetType().GetMember(enumValue.ToString())
                               .First()
                               .GetCustomAttribute<DisplayAttribute>()
                               .ShortName;

            }
            catch (Exception)
            {

                return string.Empty;
            }
        }
    }
}
