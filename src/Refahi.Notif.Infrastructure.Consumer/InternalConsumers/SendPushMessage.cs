using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Consumer.InternalConsumers
{
    public class SendPushMessageConsumer :
        IConsumer<SendPushMessage>
    {
        readonly ILogger<SendPushMessageConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IBus _bus;
        readonly PushNotificationServiceResolver _pushSender;

        public SendPushMessageConsumer(ILogger<SendPushMessageConsumer> logger, IMediator mediator, IMapper mapper, IUnitOfWork unitOfWork, PushNotificationServiceResolver pushSender, IBus bus)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _pushSender = pushSender;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<SendPushMessage> context)
        {
            try
            {
                var json = context.Message.Serilize();
                _logger.LogInformation($"start SendPushMessageConsumer Message: {json}");

                var domain = await _unitOfWork.MessageRepository.GetAsync(context.Message.MessageId);
                if (domain == null)
                {
                    _logger.LogError($"Domain Not Found For Send Push Message {json}");
                    return;
                }

                var pushNotification = domain.PushNotification;
                if (pushNotification == null)
                {
                    _logger.LogError($"PushNotification Not Found For Send Push Message {json}");
                    return;
                }
                var validatorOk = true;
                if (!string.IsNullOrEmpty(domain.ValidatorUrl))
                    validatorOk = await CheckMessageValidator(domain.ValidatorUrl);
                if (validatorOk && domain.PushNotification?.ValidatorUrl != null && domain.PushNotification?.ValidatorUrl.Length > 0)
                {
                    foreach (var url in domain.PushNotification.ValidatorUrl)
                    {
                        validatorOk = await CheckMessageValidator(url);
                        if (!validatorOk)
                            break;
                    }
                }
                if (!validatorOk)
                    domain.PushNotificationValidatorDeny();
                var exceptions = new List<Exception>();

                _logger.LogInformation($"domain: {domain.Serilize()}");
                if (validatorOk)
                {
                    if (pushNotification.Jobs == null || !pushNotification.Jobs.Any())
                    {
                        domain.PushNotificationEmpty();
                    }
                    else
                    {


                        foreach (var pushNotificationJob in pushNotification.Jobs
                                     .Where(x => x.Status == PushNotificationStatus.Pending).ToList())
                        {
                            if (pushNotificationJob.RetryCount >= 3)
                            {
                                _logger.LogInformation($"push job skiped {json}");
                                continue;
                            }

                            var done = false;
                            var result = new MessageSendingResult();
                            try
                            {
                                result = await _pushSender(pushNotificationJob.Type).Send(pushNotificationJob.Address,
                                    pushNotification.Subject, pushNotification.Body, pushNotification.Url ?? "",
                                    pushNotification.Data ?? "",
                                    context.Message.MessageId);
                                if (!result.Success && result.ErrorType ==
                                    MessageSendingErrorType.InvalidAddress)
                                {
                                    await _bus.Publish(new InvalidDevice
                                    { NotificationToken = result.NotificationToken });
                                }

                                done = true;
                                _logger.LogInformation($"push job done {json}");
                            }
                            catch (Exception ex)
                            {
                                exceptions.Add(ex);
                                _logger.LogError($"push job error  of :  {json}", ex);

                            }
                            finally
                            {
                                try
                                {
                                    if (done)
                                    {
                                        domain.PushNotificationSend(pushNotificationJob.Address, result);
                                    }
                                    else
                                    {
                                        domain.PushNotificationRetry(pushNotificationJob.Address);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError($"Error In Set PushNotification Status {json}", ex);
                                }
                            }
                        }
                    }
                }

                _unitOfWork.MessageRepository.Update(domain);
                await _unitOfWork.SaveAsync();

                if (exceptions.Any())
                    throw new AggregateException(exceptions);

                if (domain.PushNotification != null && domain.PushNotification.Jobs.Any(w => w.Status == PushNotificationStatus.Pending))
                    throw new Exception($"Jobs not completed {json}");

                _logger.LogInformation($"end Consume SendPushMessage: {json}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error In Consume SendPushMessage: {context.Message.Serilize()}");

                throw;
            }
        }


        private async Task HandleJob(PushNotification.PushNotificationJob job)
        {

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
