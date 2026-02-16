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
    public class SendMessageToUserConsumer :
        IConsumer<SendMessageToUser>
    {
        readonly ILogger<SendMessageToUserConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly SendMessageToUserRequestValidation _validator;
        public SendMessageToUserConsumer(ILogger<SendMessageToUserConsumer> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _validator = new SendMessageToUserRequestValidation();
        }
        public async Task Consume(ConsumeContext<SendMessageToUser> context)
        {
            var json = context.Message.Serilize();
            try
            {
                _logger.LogInformation($"Start Consume SendMessageToUser {json}");
                var message = _mapper.Map<SendMessageToUserRequest>(context.Message);
                if (Validate(message))
                    await _mediator.Send(message);
                _logger.LogInformation($"End Consume SendMessageToUser {json}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error In Consume SendMessageToUser: {json}");

                throw;
            }
        }

        private bool Validate(SendMessageToUserRequest message)
        {
            var validateResult = _validator.Validate(message);
            if (!validateResult.IsValid)
                _logger.LogError("Received Command Not Valid: {Text}", validateResult.JoinErrorsToString());

            return validateResult.IsValid;
        }
    }
}
