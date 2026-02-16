using System.ComponentModel.DataAnnotations;

namespace Refahi.Notif.Messages.NotifCenter.Enums
{
    public enum VerifySmsTemplate
    {
        [Display(Name = "ورود")]
        Login = 0,

        [Display(Name = "ثبت نام")]

        Register = 1,

        [Display(Name = "تغییر رمز")]

        ForgetPassword = 2
    }
}
