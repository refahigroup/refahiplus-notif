using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Contract.Repositories;
using NotificationEvent = Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities.NotificationEvent;

namespace Refahi.Notif.Application.Service.Message.Commands;

public class NotificationEventRequestHandler : IRequestHandler<NotificationEventRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<NotificationEventRequestHandler> _logger;

    public NotificationEventRequestHandler(
        IUnitOfWork unitOfWork,
        ILogger<NotificationEventRequestHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task Handle(NotificationEventRequest request, CancellationToken cancellationToken)
    {
        var e = await _unitOfWork.NotificationEventRepository.AddAsync(new NotificationEvent()
        {
            MessageId = request.MessageId,
            EventName = request.EventName,
            FCMMessageId = request.FCMMessageId,
            UserAgent = request.UserAgent,
            EventDateTime = DateTime.Now,
        });
        await _unitOfWork.SaveAsync();
    }

}