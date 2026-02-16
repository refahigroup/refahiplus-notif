using MassTransit;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Messaging.RealTime
{
    public class SendRealTimeMessageToAddressConsumer : IConsumer<SendRealTimeMessageToAddress>
    {
        readonly ILogger<SendRealTimeMessageToAddressConsumer> _logger;
        readonly IRealTimeSender _realTimeSender;
        public SendRealTimeMessageToAddressConsumer(ILogger<SendRealTimeMessageToAddressConsumer> logger, IRealTimeSender realTimeSender)
        {
            _logger = logger;
            _realTimeSender = realTimeSender;
        }

        public async Task Consume(ConsumeContext<SendRealTimeMessageToAddress> context)
        {
            var json = context.Message.Serilize();
            _logger.LogInformation($"Start Consume SendRealTimeMessageToAddress: {json}");
            try
            {
                await _realTimeSender.SendAsync(context.Message.Addresses, context.Message.Type, context.Message.Body);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Consume SendRealTimeMessageToAddress: {Message},Error:{Error}", json, ex.Message);
                throw;
            }
        }


    }
}
