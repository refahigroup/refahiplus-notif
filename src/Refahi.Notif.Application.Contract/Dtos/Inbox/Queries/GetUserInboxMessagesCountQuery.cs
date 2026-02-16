using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Inbox.Queries
{
    public class GetUserInboxMessagesCountQuery : IRequest<int>
    {
        public long UserId { get; set; }
        public AppName App { get; set; }
    }

}