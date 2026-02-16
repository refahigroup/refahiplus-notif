using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Consumer
{
    public class SmsDeliveryConsumer :
        IConsumer<SmsDeliveryChange>
    {
        readonly ILogger<SmsDeliveryChangeRequest> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        public SmsDeliveryConsumer(ILogger<SmsDeliveryChangeRequest> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<SmsDeliveryChange> context)
        {
            try
            {
                var message = _mapper.Map<SmsDeliveryChangeRequest>(context.Message);

                await _mediator.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Consume SmsDeliveryChange: {Message},Error:{Error}", context.Message.Serilize(), ex.Message);

                throw;
            }
        }

    }
}
