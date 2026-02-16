using Microsoft.Extensions.Logging;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Infrastructure.Messaging.PushNotification.APN.Library;

namespace Refahi.Notif.Infrastructure.Messaging.PushNotification.APN
{
    public class APNNotificationSender : IPushNotificationSender
    {
        private readonly APNConfiguration _config;
        private readonly HttpClient _client;
        private readonly ILogger<APNNotificationSender> _logger;
        public APNNotificationSender(
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
            try
            {
                var options = new ApnsJwtOptions()
                {
                    BundleId = _config.BundleId,
                    //CertContent = "-----BEGIN PRIVATE KEY-----\r\nMIGTA ... -----END PRIVATE KEY-----",
                    CertFilePath = _config.CertFilePath, // use either CertContent or CertFilePath, not both
                    KeyId = _config.KeyId,
                    TeamId = _config.TeamId
                };
                var apns = ApnsClient.CreateUsingJwt(new HttpClient(), options);

                var push = new ApplePush(ApplePushType.Alert)
                    .AddAlert(title, body);

                if (data != null)
                    push.CustomApsProperties = new Dictionary<string, object> { { "data", data.DeSerilize<dynamic>() } };

                if (_config.UseSandBox)
                    push.SendToDevelopmentServer();

                push.AddToken(address);



                var response = await apns.SendAsync(push);

                return new MessageSendingResult() { Message = response.ReasonString, Success = true };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
