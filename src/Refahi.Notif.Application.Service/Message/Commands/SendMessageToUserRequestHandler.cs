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
    public class SendMessageToUserRequestHandler : IRequestHandler<SendMessageToUserRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsSenderFactory _smsSenderFactory;
        private readonly ILogger<SendMessageToUserRequestHandler> _logger;
        private readonly IMessageService _messageService;
        private readonly IFileService _fileService;

        public SendMessageToUserRequestHandler(
            IUnitOfWork unitOfWork,
            IMessageService messageService,
            IFileService fileService,
            ILogger<SendMessageToUserRequestHandler> logger, ISmsSenderFactory smsSenderFactory)
        {
            _logger = logger;
            _smsSenderFactory = smsSenderFactory;
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _fileService = fileService;
        }
        public async Task Handle(SendMessageToUserRequest request, CancellationToken cancellationToken)
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

        private async Task<Domain.Core.Aggregates.MessageAgg.Message> AddDomain(SendMessageToUserRequest request)
        {
            var isExist = await _unitOfWork.MessageRepository.IsExistAsync(x => x.Id == request.Id);
            if (isExist)
            {
                _logger.LogTrace($"Message Is Exist : {request.Serilize()}");

                return null;//to be idempotent
            }

            var user = await _unitOfWork.UserRepository.GetModelAsync(request.UserId);
            if (user == null)
            {
                //todo get user from identity
                _logger.LogError($"User Not Found , Request : {request.Serilize()}");
                return null;
            }

            Sms? sms = null;

            if (request.Sms != null && !string.IsNullOrEmpty(user?.PhoneNumber))
            {
                var smsSender = _smsSenderFactory.GetService(request.Sms.Gateway);
                sms = new Sms(new[] { user.PhoneNumber }, request.Sms.Body, smsSender.Gateway, request.Sms.Sender);
            }
            var email = request.Email == null || string.IsNullOrEmpty(user?.Email) ?
                null :
                new Email(new[] { user.Email }, request.Email.Subject, request.Email.Body, request.Email.IsHtml);


            var pushnotification = request.PushNotification == null ?
                null :
                new PushNotification(user.Devices.Where(p => p.AppName == request.AppName || request.AppName == null).Select(x => new Tuple<DeviceType, string>(x.DeviceType, x.NotificationToken)).ToArray(), request.PushNotification.Subject, request.PushNotification.Body, request.PushNotification.Url, request.PushNotification.Data);

            var notification = request.Notification == null
                ? null
                : new Notification(request.Notification.Subject, request.Notification.Body,
                    request.Notification.Link,
                    request.Notification.Icon, request.Notification.ExpiredDate);

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
                                    request.UserId,
                                    request.AppName,
                                    request.DueTime,
                                    request.ValidatorUrl,
                                    sms,
                                    email,
                                    telegram,
                                    pushnotification,
                                    notification,
                                    request.Tags
                                    );


            domain = await _unitOfWork.MessageRepository.AddAsync(domain);
            await _unitOfWork.SaveAsync();
            return domain;
        }








    }
}
