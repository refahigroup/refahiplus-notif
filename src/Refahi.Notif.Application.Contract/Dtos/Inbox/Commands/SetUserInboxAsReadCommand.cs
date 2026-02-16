using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Inbox.Commands
{
    public class SetUserInboxAsReadCommand : IRequest
    {
        public SetUserInboxAsReadCommand(long userId, AppName app)
        {
            App = app;
            UserId = userId;
        }
        public AppName App { get; set; }
        public long UserId { get; set; }
    }
}