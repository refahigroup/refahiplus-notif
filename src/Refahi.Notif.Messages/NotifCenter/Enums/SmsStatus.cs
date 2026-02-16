using System.ComponentModel.DataAnnotations;

namespace Refahi.Notif.Messages.NotifCenter.Enums
{
    public enum SmsStatus
    {
        [Display(Name = "در حال پردازش")]
        Created = 0,

        [Display(Name = "در انتظار ارسال")]
        Pending = 1,

        [Display(Name = "ارسال شده")]
        Sended = 2,

        [Display(Name = "تحویل شده")]
        Delivered = 3,

        [Display(Name = "تحویل نشد")]
        UnDelivered = 4,

        [Display(Name = "غیرقابل ارسال")]
        Invalid = 5,
        [Display(Name = "لغو شده")]
        InvalidDeny = 6,
    }
}
