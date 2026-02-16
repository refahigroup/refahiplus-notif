using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Infrastructure.Messaging.Sms.KaveNegar
{
    public enum KaveSmsTemplate
    {
        OtpTemplate1,
        OtpTemplate2,
        OtpTemplate3,
        OtpTemplate4,
        OtpTemplate5,
        OtpTemplate6,
    }

    public static class KaveSmsExtenssions
    {
        public static KaveSmsTemplate GetKaveSmsTemplate(this VerifySmsTemplate template, bool isAudio)
        {
            return template switch
            {
                VerifySmsTemplate.Login => isAudio ? KaveSmsTemplate.OtpTemplate2 : KaveSmsTemplate.OtpTemplate1,
                VerifySmsTemplate.Register => isAudio ? KaveSmsTemplate.OtpTemplate4 : KaveSmsTemplate.OtpTemplate3,
                VerifySmsTemplate.ForgetPassword => isAudio ? KaveSmsTemplate.OtpTemplate6 : KaveSmsTemplate.OtpTemplate5,
                _ => throw new ArgumentException(),
            };
        }
    }
}
