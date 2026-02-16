using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Commands
{
    [AutoMap(typeof(SendMessage), ReverseMap = true)]
    public class SendMessageRequest : SendMessage, IRequest
    {

    }


}
