using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Commands
{
    [AutoMap(typeof(DeleteMessage), ReverseMap = true)]
    public class DeleteMessageRequest : DeleteMessage, IRequest
    {

    }
}
