using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;

namespace Refahi.Notif.Application.Contract.Dtos.Inbox.Queries
{
    public record GetUserInboxMessagesResult(List<InboxMessageDto> Messages);
    public class InboxMessageDto
    {
        public InboxMessageDto(Guid id, string title, string description, string link, string icon, DateTime? expiredDate, DateTime? createdTime, DateTime? readTime, DateTime? deliveredTime, InboxMessageStatus status)
        {
            Id = id;
            Title = title;
            Description = description;
            Link = link;
            Icon = icon;
            ExpiredDate = expiredDate;
            CreatedTime = createdTime;
            ReadTime = readTime;
            DeliveredTime = deliveredTime;
            Status = status;
        }
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Link { get; private set; }
        public string Icon { get; private set; }
        public DateTime? ExpiredDate { get; private set; }
        public DateTime? DeliveredTime { get; private set; }
        public DateTime? CreatedTime { get; private set; }
        public DateTime? ReadTime { get; private set; }
        public InboxMessageStatus Status { get; private set; }
        public static explicit operator InboxMessageDto(InboxMessage message)
        {
            return message is null ? null : new InboxMessageDto(
                message.Id,
                message.Title,
                message.Description,
                message.Link,
                message.Icon,
                message.ExpiredDate,
                message.CreatedTime,
                message.ReadTime,
                message.DeliveredTime,
                message.Status
            );
        }
    }
}