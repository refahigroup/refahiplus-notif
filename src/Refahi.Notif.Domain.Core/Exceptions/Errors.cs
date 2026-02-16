using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Domain.Core.Exceptions
{
    public static class Errors
    {

        public static string SmsStatusNotCorrect(SmsStatus status) => $"پیامک  {status.ToString()} است";
        public static string EmailStatusNotCorrect(EmailStatus status) => $"ایمیل  {status.ToString()} است";
        public static string TelegramStatusNotCorrect(TelegramStatus status) => $"تلگرام  {status.ToString()} است";
        public static string PushNotificationStatusNotCorrect(PushNotificationStatus status) => $"نوتیفیکیشن  {status.ToString()} است";
        public static string NotificationStatusNotCorrect(NotificationStatus status) => $"نوتیفیکیشن  {status.ToString()} است";

        public static string PushNotificationDuplicateAddress = "آدرس تکراری در نوتیفیکیشن وجود دارد";
        public static string UserIdRequired = "آی دی کاربر اجباری است";
        public static string TagRequired = "تگ اجباری است";
        public static string MessageNotFound = "رکورد یافت نشد";
        public static string PhoneNumberNotValid = "موبایل معتبر نیست";
        public static string EmailNotValid = "ایمیل معتبر نیست";
        public static string EmailExist = "ایمیل متعلق به کاربر دیگری است";
        public static string PhoneNumberExist = "شماره موبایل متعلق به کاربر دیگری است";
        public static string SmsNotFound = "پیامک یافت نشد";
        public static string SmsValidatorDeny = "پیامک لغو شده است";
        public static string EmailNotFound = "ایمیل یافت نشد";
        public static string TelegramNotFound = "تلگرام یافت نشد";
        public static string PushNotificationNotFound = "پوش نوتیفیکیشن یافت نشد";
        public static string NotificationNotFound = "نوتیفیکیشن یافت نشد";
        public static string UserIdRequiredForNotification = "آی دی کاربر برای ارسال نوتیفیکیشن اجباری است";
        public static string UserIdNotValid = "آی دی کاربر نامعتبر است";
        public static string DueTimeNotValid = "زمان ارسال نامعتبر است";
        public static string OneSendTypeMustDefined = "حداقل روش ارسال بایستی انتخاب شود";
        public static string ExpireTimeShouldGreatherThanNow = "زمان ارسال باید بزرگتر از تاریخ درخواست باشد";
        public static string PhoneNumberShould11Character = "شماره تماس باید 11 رقمی باشد";
        public static string CodeRequired = "کد اجباری است";
        public static string SmsRequestNotValid = "اطلاعات مربوط به پیامک نامعتبر است";
        public static string EmailRequestNotValid = "اطلاعات مروبط به ایمیل نامعتبر است";
        public static string PushRequestNotValid = "اطلاعات مربوط به پوش نوتیفیکیشن نامعتبر است";
        public static string NotificationRequestNotValid = "اطلاعات مربوط به نوتیفیکیشن نامعتبر است";
        public static string TelegramRequestAddressNotValid = "اطلاعات مربوط به آدرس تلگرام نامعتبر است";
        public static string TelegramRequestNotValid = "اطلاعات مربوط به تلگرام نامعتبر است";
        public static string AppNameIsRequired = "اطلاعات مربوط به اپ دریافت کننده نوتیف وارد نشده است";
        public static string MaxiOSPushIsOne = "حداکثر دستگاه های ios یکی می باشد";
        public static string UnHandledException = "خطای ناشناخته";
        public static string RealTimeMessageNotFound = "پیام ریل تایم یافت نشد";
        public static string FireBaseTokenRequired = "توکن فایربیس اجباری است";
        public static string DeviceNotFound = "دستگاه یافت نشد";


    }
}
