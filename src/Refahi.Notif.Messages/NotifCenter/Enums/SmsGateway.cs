using System.ComponentModel.DataAnnotations;

namespace Refahi.Notif.Messages.NotifCenter.Enums
{
    public enum SmsGateway
    {
        [Display(Name = "کاوه نگار")]
        Kavenegar = 1,

        [Display(Name = "نیک اس ام اس")]
        Niksms = 2,

        [Display(Name = "مدیانا")]
        Mediana = 3,

        [Display(Name = "مدیانا-هاب")]
        MedianaHub = 4,

        [Display(Name = "شاتل")]
        Shatel = 5
    }


}
