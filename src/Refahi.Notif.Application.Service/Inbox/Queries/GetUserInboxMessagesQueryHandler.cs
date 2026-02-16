using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Inbox.Queries;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.Inbox.Queries
{
    public class GetUserInboxMessagesQueryHandler : IRequestHandler<GetUserInboxMessagesQuery, GetUserInboxMessagesResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserInboxMessagesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetUserInboxMessagesResult> Handle(GetUserInboxMessagesQuery request, CancellationToken cancellationToken)
        {
            var inboxMessages = request.App == null ? await _unitOfWork.UserRepository.GetUserInboxMessages(request.UserId) :
                await _unitOfWork.UserRepository.GetUserInboxByAppMessages(request.UserId, request.App.Value, request.PageSize, request.PageNumber);
            var messageDtos = inboxMessages?.Select(m => (InboxMessageDto)m).ToList() ?? new List<InboxMessageDto>();
            var result = new GetUserInboxMessagesResult(messageDtos);
            return result;
        }
    }
}