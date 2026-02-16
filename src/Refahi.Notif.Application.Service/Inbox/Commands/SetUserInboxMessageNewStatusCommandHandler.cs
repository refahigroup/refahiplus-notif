using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Inbox;
using Refahi.Notif.Application.Contract.Dtos.Inbox.Commands;
using Refahi.Notif.Domain.Contract.Repositories;

namespace Refahi.Notif.Application.Service.Inbox.Commands;

public class SetUserInboxMessageNewStatusCommandHandler : IRequestHandler<SetUserInboxMessageNewStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    readonly IInboxMessageService _inboxMessageService;
    public SetUserInboxMessageNewStatusCommandHandler(IUnitOfWork unitOfWork, IInboxMessageService inboxMessageService)
    {
        _unitOfWork = unitOfWork;
        _inboxMessageService = inboxMessageService;
    }
    public async Task Handle(SetUserInboxMessageNewStatusCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.UserRepository.UpdateUserInboxMessageStatus(request.UserId, request.MessageId, request.NewInboxMessageStatus);
        await _unitOfWork.SaveAsync();
        await _inboxMessageService.RefreshUserUnreadInboxMessageCount(request.UserId, request.AppName);
    }

}