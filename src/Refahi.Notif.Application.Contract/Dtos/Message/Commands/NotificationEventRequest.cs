using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Commands;

[AutoMap(typeof(NotificationEvent), ReverseMap = true)]
public class NotificationEventRequest : NotificationEvent, IRequest
{
}