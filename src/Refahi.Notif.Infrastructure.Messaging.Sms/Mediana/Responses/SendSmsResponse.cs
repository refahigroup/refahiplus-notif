using Newtonsoft.Json;

namespace Refahi.Notif.Infrastructure.Messaging.Sms.Mediana.Responses
{
    public class SendSmsResponseData
    {
        [JsonProperty("message_id")]
        public long MessageId { get; set; } = 0;
    }
}
