using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Consumer;

public class NotificationEventConsumer :
    IConsumer<NotificationEvent>
{
    readonly ILogger<NotificationEventConsumer> _logger;
    readonly IMediator _mediator;
    readonly IMapper _mapper;
    public NotificationEventConsumer(ILogger<NotificationEventConsumer> logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<NotificationEvent> context)
    {
        try
        {
            var message = _mapper.Map<NotificationEventRequest>(context.Message);

            await _mediator.Send(message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error In Consume NotificationEvent: {Message},Error:{Error}", context.Message.Serilize(), ex.Message);

            throw;
        }
    }


}