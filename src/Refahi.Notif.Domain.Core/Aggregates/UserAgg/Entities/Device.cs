using Refahi.Notif.Domain.Core.Aggregates._Common;
using Refahi.Notif.Domain.Core.Exceptions;
using Refahi.Notif.Messages.NotifCenter;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities
{
    public class Device : Entity<Guid>
    {
        public string NotificationToken { get; private set; }
        public long UserId { get; private init; }
        public DeviceType Type { get; private init; }
        public AppName? App { get; private init; }
        public DateTime CreateDateTime { get; private init; }
        public DateTime? ModifiedDateTime { get; private set; }
        private Device()
        {

        }

        //id set in request to be idempotent
        internal Device(Guid id, long userId, DeviceType type, AppName? app, string firebaseToken)
        {
            if (string.IsNullOrEmpty(firebaseToken))
                throw new BussinessException(Errors.FireBaseTokenRequired);

            Id = id;
            UserId = userId;
            Type = type;
            NotificationToken = firebaseToken;
            App = app;
            CreateDateTime = DateTime.Now;
        }

        internal void UpdateFirebaseToken(string firebaseToken)
        {
            NotificationToken = firebaseToken;
            ModifiedDateTime = DateTime.Now;
        }
    }


}
