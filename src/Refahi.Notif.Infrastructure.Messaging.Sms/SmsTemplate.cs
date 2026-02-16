using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Infrastructure.Messaging.Sms
{
    public class SmsTemplate
    {
        public string LoginVerifyMessage { get; set; }
        public string RegisterVerifyMessage { get; set; }
        public string ForgetVerifyMessage { get; set; }
        public string GetVerifyMessage(VerifySmsTemplate template)
        {
            return template switch
            {
                VerifySmsTemplate.Register => RegisterVerifyMessage,
                VerifySmsTemplate.Login => LoginVerifyMessage,
                VerifySmsTemplate.ForgetPassword => ForgetVerifyMessage,
                _ => throw new ArgumentException(),
            };
        }

        public string GetVerifyMessage(VerifySmsTemplate template, string token, string serviceUrl, bool needTag)
        {
            var msg = GetVerifyMessage(template).Replace("{code}", token).Replace("{serviceUrl}", serviceUrl);
            if (!needTag)
                msg = msg.Replace("#", "");
            return msg;
        }
    }
}
