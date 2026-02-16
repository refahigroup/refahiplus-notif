using Refahi.Notif.Domain.Contract.Messaging;

namespace Refahi.Notif.Infrastructure.Messaging.RealTime
{
    public class RealTimeSender : IRealTimeSender
    {
        readonly RealTimeHub _hub;
        public RealTimeSender(RealTimeHub hub)
        {
            _hub = hub;
        }
        public async Task SendAsync(string[] connectionIds, string type, string message)
        {
            await _hub.SendMessage(connectionIds, type, message);
        }

        public async Task SendToUserAsync(long userId, string type, string message)
        {
            await _hub.SendMessageToUser(userId, type, message);
        }
    }
}
