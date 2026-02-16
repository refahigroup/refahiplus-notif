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
    public class SendMessageConsumer :
        IConsumer<SendMessage>
    {
        readonly ILogger<SendMessageConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly SendMessageRequestValidation _validator;
        public SendMessageConsumer(ILogger<SendMessageConsumer> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _validator = new SendMessageRequestValidation();
        }
        public async Task Consume(ConsumeContext<SendMessage> context)
        {
            var json = context.Message.Serilize();

            try
            {
                var message = _mapper.Map<SendMessageRequest>(context.Message);
                _logger.LogTrace($"Start Consume SendMesage : {json}");

                if (Validate(message))
                    await _mediator.Send(message);
                _logger.LogTrace($"End Consume SendMesage : {json}");
            }
            catch (Exception ex)

            {
                _logger.LogError("Error In Consume SendMessageRequest: {Message},Error:{Error}", json, ex.Message);

                throw;
            }
        }

        private bool Validate(SendMessageRequest message)
        {
            var validateResult = _validator.Validate(message);
            if (!validateResult.IsValid)
                _logger.LogError("Received Command Not Valid: {Text}", validateResult.JoinErrorsToString());

            return validateResult.IsValid;
        }
    }
}
