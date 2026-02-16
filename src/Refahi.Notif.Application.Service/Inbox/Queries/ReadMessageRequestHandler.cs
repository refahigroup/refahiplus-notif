using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Inbox.Queries;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;

namespace Refahi.Notif.Application.Service.Inbox.Queries
{
    public class GetMessagesQueryHandler : IRequestHandler<GetInboxMessageQuery, InboxMessage?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMessagesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<InboxMessage?> Handle(GetInboxMessageQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.UserRepository.GetInboxMessage(request.Id);
        }
    }
}
