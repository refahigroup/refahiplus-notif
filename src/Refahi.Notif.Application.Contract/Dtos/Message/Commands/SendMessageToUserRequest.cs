using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Commands
{
    [AutoMap(typeof(SendMessageToUser), ReverseMap = true)]
    public class SendMessageToUserRequest : SendMessageToUser, IRequest
    {

    }


}
