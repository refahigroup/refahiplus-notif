using MassTransit;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Messaging.RealTime
{
    public class SendRealTimeMessageToUserConsumer : IConsumer<SendRealTimeMessageToUser>
    {
        readonly ILogger<SendRealTimeMessageToUserConsumer> _logger;
        readonly IRealTimeSender _realTimeSender;
        public SendRealTimeMessageToUserConsumer(ILogger<SendRealTimeMessageToUserConsumer> logger, IRealTimeSender realTimeSender)
        {
            _logger = logger;
            _realTimeSender = realTimeSender;
        }

        public async Task Consume(ConsumeContext<SendRealTimeMessageToUser> context)
        {
            var json = context.Message.Serilize();
            _logger.LogInformation($"Start Consume SendRealTimeMessageToUser: {json}");
            try
            {
                await _realTimeSender.SendToUserAsync(context.Message.UserId, context.Message.Type, context.Message.Body);

                _logger.LogInformation($"End Consume SendRealTimeMessageToUser: {json}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Consume SendRealTimeMessageToUser: {Message},Error:{Error}", json, ex.Message);

                throw;
            }
        }


    }
}
