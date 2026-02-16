using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.User.Commands
{
    [AutoMap(typeof(SetUserPhoneNumber), ReverseMap = true)]
    public class SetUserPhoneNumberRequest : SetUserPhoneNumber, IRequest
    {

    }
}
