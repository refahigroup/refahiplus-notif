using Refahi.Notif.Messages.NotifCenter;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.EndPoint.Api.V1.User.Dtos
{
    public class UpsertDeviceDto
    {
        public string NotificationToken { get; set; }

        //todo get from jwt
        public DeviceType Type { get; set; }
        public Guid? DeviceId { get; set; }
        public AppName? App { get; set; }
    }
}
