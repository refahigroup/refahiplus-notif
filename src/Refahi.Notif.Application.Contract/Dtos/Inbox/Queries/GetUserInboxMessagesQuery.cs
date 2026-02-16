using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Inbox.Queries
{
    public class GetUserInboxMessagesQuery : IRequest<GetUserInboxMessagesResult>
    {
        public long UserId { get; set; }
        public AppName? App { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

}