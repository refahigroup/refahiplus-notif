using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Domain.Contract.Messaging
{
    public interface IPushNotificationSender
    {
        Task<MessageSendingResult> Send(string address, string title, string body, string url, string data,
            Guid messageMessageId);
    }
    public delegate IPushNotificationSender PushNotificationServiceResolver(DeviceType deviceType);
}
