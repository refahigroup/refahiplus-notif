using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.User.Commands;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Consumer
{
    public class FillUserInfoConsumer :
        IConsumer<FillUserInfo>
    {
        readonly ILogger<FillUserInfoConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        public FillUserInfoConsumer(ILogger<FillUserInfoConsumer> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<FillUserInfo> context)
        {
            try
            {
                var message = _mapper.Map<FillUserInfoRequest>(context.Message);

                await _mediator.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Consume FillUserInfo: {Message},Error:{Error}", context.Message.Serilize(), ex.Message);

                throw;
            }
        }

    }
}
