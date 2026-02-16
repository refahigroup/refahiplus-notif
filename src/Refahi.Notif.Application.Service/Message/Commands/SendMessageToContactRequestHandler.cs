using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Application.Service.Message.Common;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Application.Service.Message.Commands
{
    public class SendMessageToContactRequestHandler : IRequestHandler<SendMessageToContactRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsSenderFactory _smsSenderFactory;
        private readonly ILogger<SendMessageToContactRequestHandler> _logger;
        private readonly IMessageService _messageService;
        private readonly IFileService _fileService;
        private readonly IBus _bus;

        public SendMessageToContactRequestHandler(
            IUnitOfWork unitOfWork,
            IMessageService messageService,
            ILogger<SendMessageToContactRequestHandler> logger, ISmsSenderFactory smsSenderFactory, IBus bus, IFileService fileService)
        {
            _logger = logger;
            _smsSenderFactory = smsSenderFactory;
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _bus = bus;
            _fileService = fileService;

        }
        public async Task Handle(SendMessageToContactRequest request, CancellationToken cancellationToken)
        {
            var domain = await AddDomain(request);

            //for be idempotent
            if (domain == null)
            {
                return;
            }
            await _messageService.AddJobAndSetToDomainSeparate(domain);

            return;
        }

        private async Task<Domain.Core.Aggregates.MessageAgg.Message?> AddDomain(SendMessageToContactRequest request)
        {
            var isExist = await _unitOfWork.MessageRepository.IsExistAsync(x => x.Id == request.Id);
            if (isExist)
            {
                _logger.LogTrace($"Message Is Exist : {request.Serilize()}");

                return null;//to be idempotent
            }

            UserModel? user = null;
            if (request.UserId != null)
            {
                user = await _unitOfWork.UserRepository.GetModelAsync(request.UserId.Value);
                if (user == null)
                {
                    await _bus.Publish(new FillUserInfo()
                    {
                        UserId = request.UserId.Value,
                    });
                    user = new UserModel() { Id = request.UserId.Value };
                }
            }


            PushNotification? pushNotification = null;
            Notification? notification = null;
            Sms? sms = null;
            Email? email = null;
            Telegram? telegram = null;
            var dueTime = request.DueTime;

            if (request.Steps != null && request.Steps.Any())
            {
                for (var i = 0; i < request.Steps.Length; i++)
                {
                    var delayAfterPreviousStep = request.Steps[i].DelayAfterPreviousStep;
                    if (i > 0 && delayAfterPreviousStep != null &&
                        request.Steps[i].SkipIfAnyPreviousStepHandled != true)
                    {
                        dueTime ??= DateTime.Now;
                        dueTime = dueTime.Value.Add(delayAfterPreviousStep.Value);
                    }
                    string[]? dependencies = null;
                    if (i > 0 && request.Steps[i].SkipIfAnyPreviousStepHandled == true)
                    {
                        var dependenciesList = new List<string>();
                        var foundDependency = false;
                        for (int j = 0; j < i; j++)
                        {
                            switch (request.Steps[j].NotificationChannelType)
                            {
                                case NotificationChannelType.Sms:
                                    if (sms != null)
                                    {
                                        foundDependency = true;
                                        dependenciesList.Add($"http://127.0.0.1:8080/V1/MessageValidation/Check/{request.Id}/" + Enum.GetName(typeof(NotificationChannelType), request.Steps[j].NotificationChannelType));
                                    }
                                    break;
                                case NotificationChannelType.Email:
                                    if (email != null)
                                    {
                                        foundDependency = true;
                                        dependenciesList.Add($"http://127.0.0.1:8080/V1/MessageValidation/Check/{request.Id}/" + Enum.GetName(typeof(NotificationChannelType), request.Steps[j].NotificationChannelType));
                                    }
                                    break;
                                case NotificationChannelType.PushNotification:
                                    if (pushNotification != null)
                                    {
                                        foundDependency = true;
                                        dependenciesList.Add($"http://127.0.0.1:8080/V1/MessageValidation/Check/{request.Id}/" + Enum.GetName(typeof(NotificationChannelType), request.Steps[j].NotificationChannelType));
                                    }
                                    break;
                                case NotificationChannelType.Notification:
                                    if (pushNotification != null)
                                    {
                                        foundDependency = true;
                                        dependenciesList.Add($"http://127.0.0.1:8080/V1/MessageValidation/Check/{request.Id}/" + Enum.GetName(typeof(NotificationChannelType), request.Steps[j].NotificationChannelType));
                                    }
                                    break;
                                case NotificationChannelType.TelegramMessage:
                                    if (telegram != null)
                                    {
                                        foundDependency = true;
                                        dependenciesList.Add($"http://127.0.0.1:8080/V1/MessageValidation/Check/{request.Id}/" + Enum.GetName(typeof(NotificationChannelType), request.Steps[j].NotificationChannelType));
                                    }
                                    break;
                            }
                        }
                        if (foundDependency)
                        {
                            if (i > 0 && delayAfterPreviousStep != null)
                            {
                                dueTime ??= DateTime.Now;
                                dueTime = dueTime.Value.Add(delayAfterPreviousStep.Value);
                            }
                        }
                        if (dependenciesList.Any())
                            dependencies = dependenciesList.ToArray();
                    }
                    switch (request.Steps[i].NotificationChannelType)
                    {
                        case NotificationChannelType.Sms:
                            sms = HandelSms(request, dueTime, dependencies);
                            break;
                        case NotificationChannelType.Email:
                            email = HandelEmail(user, request, dueTime, dependencies);
                            break;
                        case NotificationChannelType.PushNotification:
                            pushNotification = HandelPushNotification(user, request, dueTime, dependencies);
                            break;
                        case NotificationChannelType.Notification:
                            notification = HandelNotification(request, dueTime, dependencies);
                            break;
                        case NotificationChannelType.TelegramMessage:
                            telegram = await HandelTelegram(request, dueTime, dependencies);
                            break;
                    }
                }
            }
            else
            {
                pushNotification = HandelPushNotification(user, request, dueTime, null);
                notification = HandelNotification(request, dueTime, null);
                sms = HandelSms(request, dueTime, null);
                email = HandelEmail(user, request, dueTime, null);
                telegram = await HandelTelegram(request, dueTime, null);
            }

            if (sms == null && email == null && pushNotification == null && notification == null && telegram == null)
                return null;

            var domain = new Domain.Core.Aggregates.MessageAgg.Message(
                                    request.Id,
                                    request.UserId,
                                    request.Owner,
                                    request.AppName,
                                    request.DueTime,
                                    request.ValidatorUrl,
                                    sms,
                                    email,
                                    telegram,
                                    pushNotification,
                                    notification,
                                    request.Tags
                                    );


            domain = await _unitOfWork.MessageRepository.AddAsync(domain);
            await _unitOfWork.SaveAsync();
            return domain;
        }

        private PushNotification? HandelPushNotification(UserModel? user, SendMessageToContactRequest request,
            DateTime? dueTime, string[]? stepDependencies)
        {
            if (request.PushNotification == null || user == null)
                return null;
            dueTime ??= request.PushNotification.DueTime;
            if (stepDependencies == null && request.PushNotification.DueTime != null)
                dueTime = request.PushNotification.DueTime;
            var push = new PushNotification(
                user.Devices.Where(p => p.AppName == request.AppName || request.AppName == null)
                    .Select(x => new Tuple<DeviceType, string>(x.DeviceType, x.NotificationToken)).ToArray(),
                request.PushNotification.Subject, request.PushNotification.Body, request.PushNotification.Url,
                request.PushNotification.Data, dueTime, stepDependencies);
            if (push.Jobs == null || !push.Jobs.Any())
                return null;
            return push;
        }
        private Notification? HandelNotification(SendMessageToContactRequest request, DateTime? dueTime,
            string[]? stepDependencies)
        {
            if (request.Notification == null || request.UserId == null)
                return null;
            dueTime ??= request.Notification.DueTime;
            if (stepDependencies == null && request.Notification.DueTime != null)
                dueTime = request.Notification.DueTime;
            return new Notification(request.Notification.Subject,
                request.Notification.Body,
                request.Notification.Link,
                request.Notification.Icon,
                request.Notification.ExpiredDate,
                dueTime, stepDependencies);
        }
        private async Task<Telegram?> HandelTelegram(SendMessageToContactRequest request, DateTime? dueTime,
            string[]? stepDependencies)
        {
            if (request.TelegramMessage == null)
                return null;
            string? fileId = null;
            string? fileName = null;
            if (!string.IsNullOrEmpty(request.TelegramMessage.FileName) && request.TelegramMessage.FileData != null && request.TelegramMessage.FileData.Length > 0)
            {
                fileName = request.TelegramMessage.FileName;
                fileId = await _fileService.Upload(request.TelegramMessage.FileName, request.TelegramMessage.FileData);
            }
            dueTime ??= request.TelegramMessage.DueTime;
            if (stepDependencies == null && request.TelegramMessage.DueTime != null)
                dueTime = request.TelegramMessage.DueTime;
            return new Telegram(request.TelegramMessage.ChatId,
                request.TelegramMessage.Body,
                fileName,
                fileId, dueTime, stepDependencies);
        }
        private Sms? HandelSms(SendMessageToContactRequest request, DateTime? dueTime,
            string[]? stepDependencies)
        {
            if (request.Sms == null || request.PhoneNumbers == null || !request.PhoneNumbers.Any())
                return null;
            var smsSender = _smsSenderFactory.GetService(request.Sms.Gateway);
            dueTime ??= request.Sms.DueTime;
            if (stepDependencies == null && request.Sms.DueTime != null)
                dueTime = request.Sms.DueTime;

            return new Sms(request.PhoneNumbers, request.Sms.Body, smsSender.Gateway, request.Sms.Sender, dueTime, stepDependencies);
        }
        private Email? HandelEmail(UserModel? user, SendMessageToContactRequest request, DateTime? dueTime,
            string[]? stepDependency)
        {
            if (user == null || request.Email == null || string.IsNullOrEmpty(user?.Email))
                return null;
            return new Email(new[] { user.Email }, request.Email.Subject, request.Email.Body, request.Email.IsHtml);
        }







    }
}
