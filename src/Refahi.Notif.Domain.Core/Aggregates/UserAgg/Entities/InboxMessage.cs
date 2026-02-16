using Refahi.Notif.Domain.Core.Aggregates._Common;

namespace Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;

public class InboxMessage : Entity<Guid>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string? Link { get; private set; }
    public string? Icon { get; private set; }
    public DateTime? CreatedTime { get; private set; } = DateTime.Now;
    public DateTime? DeliveredTime { get; private set; }
    public DateTime? ReadTime { get; private set; }
    public DateTime? ExpiredDate { get; private set; }
    public InboxMessageStatus Status { get; private set; } = InboxMessageStatus.None;
    public bool IsDeleted { get; private set; } = false;
    public virtual Inbox Inbox { get; set; }
    public Guid InboxId { get; set; }
    public InboxMessage()
    {
    }
    public InboxMessage(Guid id, string title, string description, string? link, string? icon, DateTime? expiredDate)
    {
        Id = id;
        Title = title;
        Description = description;
        Link = link;
        Icon = icon;
        ExpiredDate = expiredDate;
    }
    public InboxMessage(string title, string description, string? link, string? icon, DateTime? expiredDate)
    {
        Title = title;
        Description = description;
        Link = link;
        Icon = icon;
        ExpiredDate = expiredDate;
    }
    public void ReadMessage()
    {
        Status = InboxMessageStatus.Read;
        DeliveredTime ??= DateTime.Now;
        ReadTime ??= DateTime.Now;
    }
    public void SetStatus(InboxMessageStatus status)
    {
        Status = status;
        DeliveredTime ??= DateTime.Now;
        ReadTime ??= DateTime.Now;
    }
    public void DeleteMessage()
    {
        IsDeleted = true;
    }
}