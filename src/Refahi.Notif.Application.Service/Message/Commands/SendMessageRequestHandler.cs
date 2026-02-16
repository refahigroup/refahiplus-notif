using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Application.Service.Message.Common;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Application.Service.Message.Commands
{
    public class SendMessageRequestHandler : IRequestHandler<SendMessageRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsSenderFactory _smsSenderFactory;
        private readonly ILogger<SendMessageRequestHandler> _logger;
        private readonly IMessageService _messageService;
        private readonly IFileService _fileService;

        public SendMessageRequestHandler(
            IUnitOfWork unitOfWork,
            ILogger<SendMessageRequestHandler> logger,
            IMessageService messageService, ISmsSenderFactory smsSenderFactory, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _smsSenderFactory = smsSenderFactory;
            _logger = logger;
            _fileService = fileService;
        }
        public async Task Handle(SendMessageRequest request, CancellationToken cancellationToken)
        {
            var domain = await AddDomain(request);

            //for be idempotent
            if (domain == null)
            {
                return;
            }
            await _messageService.AddJobAndSetToDomain(domain);


            return;
        }
        private async Task<Domain.Core.Aggregates.MessageAgg.Message> AddDomain(SendMessageRequest request)
        {
            var isExist = await _unitOfWork.MessageRepository.IsExistAsync(x => x.Id == request.Id);
            if (isExist)
            {
                _logger.LogTrace($"Message Is Exist : {request.Serilize()}");

                return null;//to be idempotent
            }

            Sms? sms = null;

            if (request.Sms != null)
            {
                if (request.Sms.Gateway != null)
                {
                    var isSmsGatewayValid = Enum.IsDefined(typeof(SmsGateway), request.Sms.Gateway);
                    if (!isSmsGatewayValid)
                        request.Sms.Gateway = null;
                }
                var smsSender = _smsSenderFactory.GetService(request.Sms.Gateway);
                sms = new Sms(request.Sms.PhoneNumbers, request.Sms.Body, smsSender.Gateway, request.Sms.Sender);
            }

            var email = request.Email == null ?
                null :
                new Email(request.Email.Addresses, request.Email.Subject, request.Email.Body, request.Email.IsHtml);


            var pushnotification = request.PushNotification == null ?
                null :
                new PushNotification(request.PushNotification.Addresses.Select(x => new Tuple<DeviceType, string>(x.DeviceType, x.Address)).ToArray(), request.PushNotification.Subject, request.PushNotification.Body, request.PushNotification.Url, request.PushNotification.Data);

            Telegram? telegram = null;
            if (request.Telegram != null)
            {
                string? fileId = null;
                if (!string.IsNullOrEmpty(request.Telegram.FileName) && request.Telegram.FileData != null && request.Telegram.FileData.Length > 0)
                {
                    fileId = await _fileService.Upload(request.Telegram.FileName, request.Telegram.FileData);
                }
                telegram = new Telegram(request.Telegram.ChatId,
                    request.Telegram.Body,
                    request.Telegram.FileName, fileId);
            }
            var domain = new Domain.Core.Aggregates.MessageAgg.Message(
                                    request.Id,
                                    request.DueTime,
                                    request.ValidatorUrl,
                                    sms,
                                    email,
                                    telegram,
                                    pushnotification,
                                    request.Tags.ToList()
                                    );


            domain = await _unitOfWork.MessageRepository.AddAsync(domain);
            await _unitOfWork.SaveAsync();
            return domain;
        }

    }
}
