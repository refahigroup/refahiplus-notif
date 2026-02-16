using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Messages.NotifCenter
{
    public class VerifySmsSent
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public VerifySmsTemplate Template { get; set; }
        public DateTime SendTime { get; set; }
        public string IdInProvider { get; set; }
        public bool IsAudio { get; set; }
    }
}
