using Refahi.Notif.Messages.Communication.Enums;

namespace Refahi.Notif.Messages.Communication
{
    public class AddCall
    {
        public Guid Id { get; set; }
        public long CallerUserId { get; set; }
        public Guid CallerDeviceId { get; set; }
        public long RecieverUserId { get; set; }
        public CallType Type { get; set; }
    }
}
