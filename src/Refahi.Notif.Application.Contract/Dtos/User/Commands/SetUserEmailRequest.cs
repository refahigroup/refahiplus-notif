using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.User.Commands
{
    [AutoMap(typeof(SetUserEmail), ReverseMap = true)]
    public class SetUserEmailRequest : SetUserEmail, IRequest
    {

    }
}
