using Microsoft.Extensions.Logging;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;
using Refahi.Notif.Domain.Core.Utility;

namespace Refahi.Notif.Infrastructure.Messaging.PushNotification.APN
{
    public class APNProxyNotificationSender : IPushNotificationSender
    {
        private readonly APNConfiguration _config;
        private readonly HttpClient _client;
        private readonly ILogger<APNNotificationSender> _logger;
        public APNProxyNotificationSender(
            APNConfiguration config,
            ILogger<APNNotificationSender> logger,
            HttpClient client)
        {
            _config = config;
            _client = client;
            _logger = logger;
        }

        public async Task<MessageSendingResult> Send(string address, string title, string body, string url,
            string data, Guid messageMessageId)
        {
            var res = _client.PostString(_config.ProxyUrl, new
            {
                addresses = new string[] { address },
                title,
                body,
                data
            }, new Dictionary<string, string>
            {
                { "secret", _config.ProxySecret },
            });

            return new MessageSendingResult() { };
        }

    }
}
