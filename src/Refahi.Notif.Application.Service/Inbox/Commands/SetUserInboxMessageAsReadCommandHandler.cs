using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Inbox;
using Refahi.Notif.Application.Contract.Dtos.Inbox.Commands;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;

namespace Refahi.Notif.Application.Service.Inbox.Commands
{
    public class SetUserInboxMessageAsReadCommandHandler : IRequestHandler<SetUserInboxMessageAsReadCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        readonly IInboxMessageService _inboxMessageService;
        public SetUserInboxMessageAsReadCommandHandler(IUnitOfWork unitOfWork, IInboxMessageService inboxMessageService)
        {
            _unitOfWork = unitOfWork;
            _inboxMessageService = inboxMessageService;
        }
        public async Task Handle(SetUserInboxMessageAsReadCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository.UpdateUserInboxMessageStatus(request.UserId, request.MessageId, InboxMessageStatus.Read);
            await _unitOfWork.SaveAsync();
            await _inboxMessageService.RefreshUserUnreadInboxMessageCount(request.UserId, request.AppName);
        }
    }
}