
using Refahi.Notif.Domain.Core.Aggregates._Common;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;
using Refahi.Notif.Domain.Core.Exceptions;
using Refahi.Notif.Messages.NotifCenter;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Domain.Core.Aggregates.UserAgg
{
    public class User : AggregateRoot<long>
    {
        public List<Device> Devices { get; private set; } = new();
        public List<Inbox> Inboxes { get; private set; } = new();
        public string? PhoneNumber { get; private set; }
        public string? Email { get; private set; }
        private User()
        {

        }
        public User(long id)
        {
            Id = id;
        }

        public void UpsertDevice(Guid deviceId, string firebaseToken, DeviceType type, AppName? app)
        {
            if (Devices.Any(x => x.Id == deviceId))
            {
                var device = Devices.First(x => x.Id == deviceId);
                device.UpdateFirebaseToken(firebaseToken);
                return;
            }
            if (Devices.Any(x => x.Id != deviceId && x.NotificationToken == firebaseToken))
            {
                var device = Devices.First(x => x.Id != deviceId && x.NotificationToken == firebaseToken);
                Devices.Remove(device);
            }

            Devices.Add(new Device(deviceId, Id, type, app, firebaseToken));

        }


        public void RemoveDevice(Guid deviceId)
        {
            var device = Devices.FirstOrDefault(x => x.Id == deviceId);
            if (device == null)
                throw new BussinessException(Errors.DeviceNotFound);

            Devices.Remove(device);
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public void SetEmail(string email)
        {
            Email = email;
        }
    }
}
