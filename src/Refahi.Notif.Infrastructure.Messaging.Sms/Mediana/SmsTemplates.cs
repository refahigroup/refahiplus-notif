namespace Refahi.Notif.Infrastructure.Messaging.Sms.Mediana;

/// <summary>
/// SMS message templates for different scenarios
/// Centralized location for all SMS content management
/// </summary>
public static class SmsTemplates
{
    /// <summary>
    /// OTP verification code template for registration/login
    /// </summary>
    /// <param name="code">6-digit OTP code</param>
    /// <param name="validMinutes">Validity duration in minutes (default: 5)</param>
    public static string VerificationCode(string code, int validMinutes = 5)
    {
        return $@"کد تایید شما: {code}

این کد برای {validMinutes} دقیقه معتبر است.

کاهن AI - دستیار هوش مصنوعی";
    }

    /// <summary>
    /// Welcome message after successful registration
    /// </summary>
    /// <param name="username">User's username</param>
    public static string Welcome(string username)
    {
        return $@"سلام {username}!

به کاهن AI خوش آمدید.
با ما دنیای جدیدی از هوش مصنوعی را کشف کنید.

کاهن AI";
    }

    /// <summary>
    /// Password reset OTP template
    /// </summary>
    /// <param name="code">Reset code</param>
    public static string PasswordReset(string code)
    {
        return $@"کد بازیابی رمز عبور: {code}

این کد برای 10 دقیقه معتبر است.
اگر درخواست بازیابی نداشتید، این پیام را نادیده بگیرید.

کاهن AI";
    }

    /// <summary>
    /// Security alert when login from new device
    /// </summary>
    /// <param name="deviceInfo">Device or location info</param>
    public static string SecurityAlert(string deviceInfo)
    {
        return $@"ورود جدید به حساب شما:
{deviceInfo}

اگر این شما نبودید، فوراً رمز عبور خود را تغییر دهید.

کاهن AI";
    }

    /// <summary>
    /// Generic notification template
    /// </summary>
    /// <param name="title">Notification title</param>
    /// <param name="message">Notification message</param>
    public static string Notification(string title, string message)
    {
        return $@"{title}

{message}

کاهن AI";
    }
}
