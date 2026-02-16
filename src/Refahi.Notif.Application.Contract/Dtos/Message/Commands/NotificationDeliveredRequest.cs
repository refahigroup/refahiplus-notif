using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Commands
{
    [AutoMap(typeof(NotificationDelivered), ReverseMap = true)]
    public class NotificationDeliveredRequest : NotificationDelivered, IRequest
    {
    }
}
