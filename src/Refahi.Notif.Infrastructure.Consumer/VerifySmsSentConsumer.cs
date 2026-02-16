using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.VerifySms.Commands;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Consumer
{
    public class VerifySmsSentConsumer :
        IConsumer<VerifySmsSent>
    {
        readonly ILogger<VerifySmsSentConsumer> _logger;
        readonly IMediator _mediator;
        readonly IMapper _mapper;
        readonly IBus _bus;
        public VerifySmsSentConsumer(ILogger<VerifySmsSentConsumer> logger, IMediator mediator, IMapper mapper, IBus bus)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<VerifySmsSent> context)
        {
            var json = context.Message.Serilize();
            _logger.LogInformation($"Start Consume VerifySmsSent : {json}");
            try
            {
                var message = _mapper.Map<AddVerifyMessageRequest>(context.Message);


                await _mediator.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Consume VerifySmsSent: {Message},Error:{Error}", json, ex.Message);

                throw;
            }
            _logger.LogInformation($"End Consume Message : {json}");



        }


    }
}
