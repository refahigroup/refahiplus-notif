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
    public class DeleteMessageByTagConsumer :
        IConsumer<DeleteMessageByTag>
    {
        readonly ILogger<DeleteMessageByTagConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly DeleteMessageByTagRequestValidation _validator;
        public DeleteMessageByTagConsumer(ILogger<DeleteMessageByTagConsumer> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _validator = new();
        }

        public async Task Consume(ConsumeContext<DeleteMessageByTag> context)
        {
            try
            {
                var message = new DeleteMessageByTagRequest() { Tag = context.Message.Tag, DeleteDirect = false };
                if (Validate(message))
                    await _mediator.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Consume DeleteMessageByTag: {Message},Error:{Error}", context.Message.Serilize(), ex.Message);

                throw;
            }
        }
        private bool Validate(DeleteMessageByTagRequest message)
        {
            var validateResult = _validator.Validate(message);
            if (!validateResult.IsValid)
                _logger.LogError("Received Command Not Valid: {Text}", validateResult.JoinErrorsToString());

            return validateResult.IsValid;
        }

    }
}
