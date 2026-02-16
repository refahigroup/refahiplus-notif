using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Commands
{
    [AutoMap(typeof(NotificationClicked), ReverseMap = true)]
    public class NotificationClickedRequest : NotificationClicked, IRequest
    {

    }
}
