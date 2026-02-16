using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Inbox;
using Refahi.Notif.Application.Contract.Dtos.Inbox.Queries;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.Inbox.Queries
{
    public class GetUserInboxMessagesCountQueryHandler : IRequestHandler<GetUserInboxMessagesCountQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInboxMessageService _inboxMessageService;
        public GetUserInboxMessagesCountQueryHandler(IUnitOfWork unitOfWork, IInboxMessageService inboxMessageService)
        {
            _unitOfWork = unitOfWork;
            _inboxMessageService = inboxMessageService;
        }
        public async Task<int> Handle(GetUserInboxMessagesCountQuery request, CancellationToken cancellationToken)
        {
            return await _inboxMessageService.GetUserUnreadInboxMessageCount(request.UserId, request.App);
        }
    }
}