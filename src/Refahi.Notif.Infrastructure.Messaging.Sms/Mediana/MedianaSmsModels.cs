using System.Text.Json.Serialization;

namespace Refahi.Notif.Infrastructure.Messaging.Sms.Mediana;

/// <summary>
/// Request model for sending Normal SMS (API v1)
/// Endpoint: POST /sms/v1/send/sms
/// </summary>
internal class MedianaSendSmsRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Informational";

    [JsonPropertyName("recipients")]
    public string[] Recipients { get; set; } = Array.Empty<string>();

    [JsonPropertyName("messageText")]
    public string MessageText { get; set; } = string.Empty;
}

/// <summary>
/// Request model for sending Pattern SMS (API v1)
/// Endpoint: POST /sms/v1/send/pattern
/// </summary>
internal class MedianaSendPatternRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Informational";

    [JsonPropertyName("recipients")]
    public string[] Recipients { get; set; } = Array.Empty<string>();

    [JsonPropertyName("patternCode")]
    public string PatternCode { get; set; } = string.Empty;

    [JsonPropertyName("parameters")]
    public Dictionary<string, string> Parameters { get; set; } = new();
}

/// <summary>
/// Response model from Mediana SMS API (API v1)
/// </summary>
internal class MedianaSmsResponse
{
    [JsonPropertyName("meta")]
    public MedianaResponseMeta? Meta { get; set; }

    [JsonPropertyName("data")]
    public MedianaResponseData? Data { get; set; }
}

internal class MedianaResponseMeta
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }
}

internal class MedianaResponseData
{
    [JsonPropertyName("succeed")]
    public bool Succeed { get; set; }

    [JsonPropertyName("requestId")]
    public int RequestId { get; set; }

    [JsonPropertyName("requestCode")]
    public string? RequestCode { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("totalPrice")]
    public int? TotalPrice { get; set; }
}
