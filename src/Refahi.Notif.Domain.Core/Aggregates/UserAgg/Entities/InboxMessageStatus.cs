namespace Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;

public enum InboxMessageStatus
{
    None = 0,
    Delivered = 1,
    Read = 2,
    Hide = 3,
    Done = 4,
}