using System.Text.Json.Serialization;

namespace Refahi.Notif.Infrastructure.Messaging.Sms.MedianaSMSHub;

public class MedianaHubTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expires_at")]
    public DateTimeOffset Expires { get; set; }
}

public class MedianaHubSendSmsRequest
{
    [JsonPropertyName("sourceAddress")]
    public string SourceAddress { get; set; }

    [JsonPropertyName("destinationAddress")]
    public string[] DestinationAddress { get; set; }

    [JsonPropertyName("validityPeriod")]
    public DateTime ValidityPeriod { get; set; }

    [JsonPropertyName("messageText")]
    public string MessageText { get; set; }
}

public class MedianaHubSendSmsResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("succeeded")]
    public bool Succeeded { get; set; }

    [JsonPropertyName("data")]
    public List<string> IdList { get; set; }

    [JsonPropertyName("resultCode")]
    public int ResultCode { get; set; }
}