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
    public class SendMessageToContactConsumer :
        IConsumer<SendMessageToContact>
    {
        readonly ILogger<SendMessageToContactConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly SendMessageToContactRequestValidation _validator;
        public SendMessageToContactConsumer(ILogger<SendMessageToContactConsumer> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _validator = new SendMessageToContactRequestValidation();
        }
        public async Task Consume(ConsumeContext<SendMessageToContact> context)
        {
            var json = context.Message.Serilize();
            try
            {
                _logger.LogInformation($"Start Consume SendMessageToContact {json}");
                var message = _mapper.Map<SendMessageToContactRequest>(context.Message);
                if (Validate(message))
                    await _mediator.Send(message);
                _logger.LogInformation($"End Consume SendMessageToContact {json}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error In Consume SendMessageToContact: {json}");

                throw;
            }
        }

        private bool Validate(SendMessageToContactRequest message)
        {
            var validateResult = _validator.Validate(message);
            if (!validateResult.IsValid)
                _logger.LogError("Received Command Not Valid: {Text}", validateResult.JoinErrorsToString());

            return validateResult.IsValid;
        }
    }
}
