using MediatR;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Inbox.Commands
{
    public class SetUserInboxMessageAsReadCommand : IRequest
    {
        public SetUserInboxMessageAsReadCommand(Guid messageId, long userId, AppName appName)
        {
            MessageId = messageId;
            UserId = userId;
            AppName = appName;
        }
        public Guid MessageId { get; set; }
        public long UserId { get; set; }
        public AppName AppName { get; set; }
    }
    public class SetUserInboxMessageNewStatusCommand : IRequest
    {
        public SetUserInboxMessageNewStatusCommand(Guid messageId, long userId, AppName appName, InboxMessageStatus newInboxMessageStatus)
        {
            MessageId = messageId;
            UserId = userId;
            AppName = appName;
            NewInboxMessageStatus = newInboxMessageStatus;
        }
        public Guid MessageId { get; set; }
        public long UserId { get; set; }
        public AppName AppName { get; set; }
        public InboxMessageStatus NewInboxMessageStatus { get; set; }
    }
}