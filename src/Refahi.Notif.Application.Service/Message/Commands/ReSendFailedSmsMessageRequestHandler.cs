using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Application.Service.Message.Common;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Service.Message.Commands
{
    public class ReSendFailedSmsMessageRequestHandler : IRequestHandler<ReSendFailedSmsMessageRequest, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBus _bus;
        private readonly ILogger<ReSendFailedSmsMessageRequestHandler> _logger;
        private readonly IMessageService _messageService;

        public ReSendFailedSmsMessageRequestHandler(
            IUnitOfWork unitOfWork,
            IBus bus,
            ILogger<ReSendFailedSmsMessageRequestHandler> logger,
            IMessageService messageService
            )
        {
            _bus = bus;
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _logger = logger;
        }
        public async Task<int> Handle(ReSendFailedSmsMessageRequest request, CancellationToken cancellationToken)
        {
            var messageIds = await _unitOfWork.MessageRepository.SmsPendingRetried(request.MinRetryCount, request.StartTime, request.EndTime, request.Ids);

            await _bus.PublishBatch(messageIds.Select(x => new SendSmsMessage { MessageId = x }));
            return messageIds.Count;
        }

    }
}
