using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.User.Commands
{
    [AutoMap(typeof(InvalidDevice), ReverseMap = true)]
    public class InvalidDeviceRequest : InvalidDevice, IRequest
    {

    }
}
