using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;

namespace Refahi.Notif.Domain.Contract.Repositories;

public interface INotificationEventRepository
{
    Task<NotificationEvent> AddAsync(NotificationEvent domain);
    Task<List<NotificationEvent>> GetListByNotificationIdAsync(Guid messageId);
}