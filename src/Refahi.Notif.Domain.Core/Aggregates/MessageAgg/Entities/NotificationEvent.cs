using Refahi.Notif.Domain.Core.Aggregates._Common;

namespace Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;

public class NotificationEvent : AggregateRoot<long>
{
    public Guid? MessageId { get; set; }
    public string? FCMMessageId { get; set; }
    public string? UserAgent { get; set; }
    public string? EventName { get; set; }
    public DateTime? EventDateTime { get; set; }
}