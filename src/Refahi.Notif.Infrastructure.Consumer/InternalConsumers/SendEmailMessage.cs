using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Consumer.InternalConsumers
{
    public class SendEmailMessageConsumer :
        IConsumer<SendEmailMessage>
    {
        readonly ILogger<SendEmailMessageConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IEmailSender _emailSender;

        public SendEmailMessageConsumer(ILogger<SendEmailMessageConsumer> logger, IMediator mediator, IMapper mapper, IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public async Task Consume(ConsumeContext<SendEmailMessage> context)
        {
            try
            {
                var domain = await _unitOfWork.MessageRepository.GetAsync(context.Message.MessageId);
                if (domain == null || domain.Email?.RetryCount >= 3)
                    return;

                var email = domain.Email;
                var done = false;
                try
                {
                    await _emailSender.Send(email.Addresses, email.Subject, email.Body, email.IsHtml);
                    done = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error In Send Email");
                    throw;
                }
                finally
                {
                    try
                    {
                        if (done)
                            domain.EmailSend();
                        else
                            domain.EmailRetry();
                        _unitOfWork.MessageRepository.Update(domain);
                        await _unitOfWork.SaveAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error In Set Email Status : {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Consume SendEmailMessage: {Message},Error:{Error}", context.Message.Serilize(), ex.Message);

                throw;
            }
        }


    }
}
