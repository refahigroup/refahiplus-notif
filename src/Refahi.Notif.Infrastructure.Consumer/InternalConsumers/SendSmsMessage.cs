using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Infrastructure.Consumer.InternalConsumers
{
    public class SendSmsMessageConsumer :
        IConsumer<SendSmsMessage>
    {
        readonly ILogger<SendSmsMessageConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly ISmsSenderFactory _smsSenderFactory;

        public SendSmsMessageConsumer(
            ILogger<SendSmsMessageConsumer> logger, 
            IMediator mediator, IMapper mapper, 
            IUnitOfWork unitOfWork, 
            ISmsSenderFactory smsSenderFactory)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _smsSenderFactory = smsSenderFactory;
        }

        public async Task Consume(ConsumeContext<SendSmsMessage> context)
        {
            try
            {
                var domain = await _unitOfWork.MessageRepository.GetAsync(context.Message.MessageId);
                if (domain == null || domain.Sms?.Status != SmsStatus.Pending)
                    return;

                var sms = domain.Sms;
                var done = false;
                var isInvalid = false;
                var validatorOk = true;
                string idInProvider = null;
                try
                {
                    if (!string.IsNullOrEmpty(domain.ValidatorUrl))
                        validatorOk = await CheckSmsValidator(domain.ValidatorUrl);
                    if (validatorOk && domain.Sms?.ValidatorUrl != null && domain.Sms?.ValidatorUrl.Length > 0)
                    {
                        foreach (var url in domain.Sms.ValidatorUrl)
                        {
                            validatorOk = await CheckSmsValidator(url);
                            if (!validatorOk)
                                break;
                        }
                    }

                    if (validatorOk)
                    {
                        if (string.IsNullOrEmpty(sms.Body) || !sms.PhoneNumbers.Any())
                        {
                            isInvalid = true;
                        }
                        else
                        {
                            var smsSender = _smsSenderFactory.GetService(domain.Sms.Gateway);
                            var sendResult = await smsSender.SendAsync(sms.PhoneNumbers, sms.Body, sms.Sender);
                            if (sendResult.Item2)
                                isInvalid = true;
                            else
                                idInProvider = sendResult.Item1;
                        }
                        done = true;
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error In Set Sms Status : {ex.Message}");
                    throw;
                }
                finally
                {
                    try
                    {
                        if (!validatorOk)
                            domain.SmsValidatorDeny();
                        else if (isInvalid)
                            domain.SmsInvalid();
                        else if (done)
                            domain.SmsSend(idInProvider);
                        else
                            domain.SmsRetry();
                        _unitOfWork.MessageRepository.Update(domain);
                        await _unitOfWork.SaveAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error In Set Sms Status : {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Consume SendSmsMessageConsumer: {Message},Error:{Error}", context.Message.Serilize(), ex.GetFullError());

                throw;
            }
        }

        private static async Task<bool> CheckSmsValidator(string url)
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
