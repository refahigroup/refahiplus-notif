using MediatR;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;

namespace Refahi.Notif.Application.Contract.Dtos.Inbox.Queries
{
    public class GetInboxMessageQuery : IRequest<InboxMessage?>
    {
        public Guid Id { get; set; }
    }

}