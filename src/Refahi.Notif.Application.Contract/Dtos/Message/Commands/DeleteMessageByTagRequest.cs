using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Commands
{
    [AutoMap(typeof(DeleteMessageByTag), ReverseMap = true)]
    public class DeleteMessageByTagRequest : DeleteMessageByTag, IRequest
    {

    }
}
