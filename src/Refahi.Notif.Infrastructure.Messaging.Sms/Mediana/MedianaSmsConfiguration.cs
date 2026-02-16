namespace Refahi.Notif.Infrastructure.Messaging.Sms.Mediana;

public class MedianaSmsConfiguration
{
    private string _apiKey = string.Empty;

    public const string SectionName = "MedianaSmsConfiguration";

    public string ApiKey 
    {
        get => _apiKey;
        set
        {
            _apiKey = value.Replace("{SMS_API_KEY}", Environment.GetEnvironmentVariable("SMS_API_KEY") ?? "");
        }
    }

    public string BaseUrl { get; set; } = "https://api.mediana.ir";

    public string MessageType { get; set; } = "Informational";

    public bool IsDefaultGateway { get; set; } = false;
}
