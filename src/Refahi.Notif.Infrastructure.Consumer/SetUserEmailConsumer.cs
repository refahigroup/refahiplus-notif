using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.User.Commands;
using Refahi.Notif.Application.Contract.Dtos.User.Validation;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Consumer
{
    public class SetUserEmailConsumer :
        IConsumer<SetUserEmail>
    {
        readonly ILogger<SetUserEmailConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly SetUserEmailRequestValidation _validator;
        public SetUserEmailConsumer(ILogger<SetUserEmailConsumer> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _validator = new SetUserEmailRequestValidation();
        }
        public async Task Consume(ConsumeContext<SetUserEmail> context)
        {
            try
            {
                var message = _mapper.Map<SetUserEmailRequest>(context.Message);
                if (Validate(message))
                    await _mediator.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Consume SetUserEmail: {Message},Error:{Error}", context.Message.Serilize(), ex.Message);

                throw;
            }
        }

        private bool Validate(SetUserEmailRequest message)
        {
            var validateResult = _validator.Validate(message);
            if (!validateResult.IsValid)
                _logger.LogError("Received Command Not Valid: {Text}", validateResult.JoinErrorsToString());

            return validateResult.IsValid;
        }
    }
}
