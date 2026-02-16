namespace Refahi.Notif.Infrastructure.Messaging.Sms.MedianaSMSHub;

public class MedianaHubSmsConfiguration
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Scope { get; set; }
    public string Sender { get; set; }
    public string BaseUrl { get; set; }
    public string SendSmsAction { get; set; }
    public string GetTokenAction { get; set; }

}