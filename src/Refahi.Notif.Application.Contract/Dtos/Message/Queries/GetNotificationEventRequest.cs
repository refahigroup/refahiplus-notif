using AutoMapper;
using Refahi.Notif.Messages.NotifCenter;
using MediatR;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Queries;

[AutoMap(typeof(NotificationEvent), ReverseMap = true)]
public class GetNotificationEventRequest : IRequest<List<Domain.Core.Aggregates.MessageAgg.Entities.NotificationEvent>>
{
    public Guid Id { get; set; }
}