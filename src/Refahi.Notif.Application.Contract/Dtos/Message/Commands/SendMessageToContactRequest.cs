using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Commands
{
    [AutoMap(typeof(SendMessageToContact), ReverseMap = true)]
    public class SendMessageToContactRequest : SendMessageToContact, IRequest
    {

    }


}
