using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Messages.NotifCenter
{
    public class SendVerifySms
    {
        public DateTime ExpireTime { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public bool IsResend { get; set; }
        public bool IsAudio { get; set; }
        public VerifySmsTemplate Template { get; set; }
        public string? ServiceUrl { get; set; }
        public bool? NeedTag { get; set; }
    }
}