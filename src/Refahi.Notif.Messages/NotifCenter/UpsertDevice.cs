using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Messages.NotifCenter
{

    public class UpsertDevice
    {
        public Guid DeviceId { get; set; }
        public long UserId { get; set; }
        public string NotificationToken { get; set; }
        public DeviceType Type { get; set; }
        public AppName? App { get; set; }
    }
}
