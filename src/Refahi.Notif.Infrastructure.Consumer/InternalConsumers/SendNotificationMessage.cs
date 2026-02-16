using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.Inbox;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Consumer.InternalConsumers
{
    public class SendNotificationMessageConsumer :
        IConsumer<SendNotificationMessage>
    {
        readonly ILogger<SendNotificationMessageConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IInboxMessageService _inboxMessageService;


        public SendNotificationMessageConsumer(ILogger<SendNotificationMessageConsumer> logger, IMediator mediator, IMapper mapper, IUnitOfWork unitOfWork, IInboxMessageService InboxMessageService)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _inboxMessageService = InboxMessageService;
        }

        public async Task Consume(ConsumeContext<SendNotificationMessage> context)
        {
            try
            {
                var domain = await _unitOfWork.MessageRepository.GetAsync(context.Message.MessageId);
                if (domain == null || domain.Notification == null)
                    return;
                if (domain.UserId == null || domain.AppName == null)
                    return;
                var validatorOk = true;
                if (!string.IsNullOrEmpty(domain.ValidatorUrl))
                    validatorOk = await CheckMessageValidator(domain.ValidatorUrl);
                if (validatorOk && domain.Notification?.ValidatorUrl != null && domain.Notification?.ValidatorUrl.Length > 0)
                {
                    foreach (var url in domain.Notification.ValidatorUrl)
                    {
                        validatorOk = await CheckMessageValidator(url);
                        if (!validatorOk)
                            break;
                    }
                }
                if (!validatorOk)
                    domain.NotificationValidatorDeny();
                else
                {
                    await _unitOfWork.UserRepository.AddMessageToUserInbox(domain.UserId.Value, domain.AppName.Value,
                        new InboxMessage(domain.Id, domain.Notification.Subject, domain.Notification.Body, domain.Notification.Link,
                            domain.Notification.Icon, domain.Notification.ExpiredDate)
                    );
                    domain.NotificationSend();
                    await _inboxMessageService.IncreaseUserUnreadInboxMessageCount(domain.UserId.Value, domain.AppName.Value);
                }

                _unitOfWork.MessageRepository.Update(domain);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Consume SendNotificationMessage: {Message},Error:{Error}", context.Message.Serilize(), ex.Message);
                throw;
            }
        }
        private static async Task<bool> CheckMessageValidator(string url)
        {
            if (string.IsNullOrEmpty(url))
                return true;
            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }


    }
}
