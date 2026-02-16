using Refahi.Notif.Domain.Core.Aggregates.UserAgg;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;
using Refahi.Notif.Messages.NotifCenter;
using Refahi.Notif.Messages.NotifCenter.Enums;
using System.Linq.Expressions;

namespace Refahi.Notif.Domain.Contract.Models
{
    public class UserModel
    {

        public static Expression<Func<User, UserModel>> UserExpression = x => new UserModel
        {
            Devices = x.Devices.Select(x => new DeviceModel(x)).ToList(),
            Email = x.Email,
            Id = x.Id,
            PhoneNumber = x.PhoneNumber,
        };
        public long Id { get; set; }
        public List<DeviceModel> Devices { get; set; } = new();
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }

    public class DeviceModel
    {
        public DeviceModel(Device domain)
        {
            Id = domain.Id;
            NotificationToken = domain.NotificationToken;
            DeviceType = domain.Type;
            AppName = domain.App;
        }
        public Guid Id { get; set; }
        public DeviceType DeviceType { get; set; }
        public AppName? AppName { get; set; }
        public string NotificationToken { get; set; }
    }

}
