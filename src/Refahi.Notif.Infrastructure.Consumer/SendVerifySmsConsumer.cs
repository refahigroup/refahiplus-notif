using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Application.Contract.Dtos.Message.Validation;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Consumer
{
    public class SendVerifySmsConsumer : IConsumer<SendVerifySms>
    {
        readonly ILogger<SendVerifySmsConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly IBus _bus;
        readonly SendVerifySmsRequestValidation _validator;
        public SendVerifySmsConsumer(ILogger<SendVerifySmsConsumer> logger, IMediator mediator, IMapper mapper, IBus bus)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _validator = new SendVerifySmsRequestValidation();
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<SendVerifySms> context)
        {
            var json = context.Message.Serilize();
            _logger.LogInformation($"Start Consume Message : {json}");
            string idInProvider = null;
            try
            {
                SendVerifySmsRequest message = _mapper.Map<SendVerifySmsRequest>(context.Message);
                if (message.ExpireTime <= DateTime.Now)
                {
                    _logger.LogWarning($"Verify Message Expired : {json}");
                    return;
                }

                if (Validate(message))
                    idInProvider = await _mediator.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Consume SendVerifySmsRequest: {Message},Error:{Error}", json, ex.Message);

                throw;
            }
            _logger.LogInformation($"End Consume Message : {json}");

            if (idInProvider != null)
                try
                {
                    await _bus.Publish(new VerifySmsSent
                    {
                        Id = Guid.NewGuid(),
                        Template = context.Message.Template,
                        IdInProvider = idInProvider,
                        IsAudio = context.Message.IsAudio,
                        PhoneNumber = context.Message.PhoneNumber,
                        SendTime = DateTime.Now
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error In Send Event VerifySmsSent");
                }

        }

        private bool Validate(SendVerifySmsRequest message)
        {
            var validateResult = _validator.Validate(message);
            if (!validateResult.IsValid)
                _logger.LogError("Received Command Not Valid: {Text}", validateResult.JoinErrorsToString());

            return validateResult.IsValid;
        }
    }
}
