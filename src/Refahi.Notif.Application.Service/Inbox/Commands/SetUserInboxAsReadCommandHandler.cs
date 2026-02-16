using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Inbox;
using Refahi.Notif.Application.Contract.Dtos.Inbox.Commands;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.Inbox.Commands
{
    public class SetUserInboxAsReadCommandHandler : IRequestHandler<SetUserInboxAsReadCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInboxMessageService _inboxMessageService;
        public SetUserInboxAsReadCommandHandler(IUnitOfWork unitOfWork, IInboxMessageService inboxMessageService)
        {
            _unitOfWork = unitOfWork;
            _inboxMessageService = inboxMessageService;
        }
        public async Task Handle(SetUserInboxAsReadCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository.UpdateUserInboxMessageStatus(request.UserId, request.App);
            await _unitOfWork.SaveAsync();
            await _inboxMessageService.ResetUserUnreadInboxMessageCount(request.UserId, request.App);
        }

    }
}