using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Inbox;

public interface IInboxMessageService
{
    Task IncreaseUserUnreadInboxMessageCount(long userId, AppName appName);
    Task RefreshUserUnreadInboxMessageCount(long userId, AppName appName);
    Task ResetUserUnreadInboxMessageCount(long userId, AppName appName);
    Task<int> GetUserUnreadInboxMessageCount(long userId, AppName appName);
}