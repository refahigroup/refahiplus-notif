namespace Refahi.Notif.Messages.NotifCenter;

public class NotificationEvent
{
    public Guid? MessageId { get; set; }
    public string? FCMMessageId { get; set; }
    public string? UserAgent { get; set; }
    public string? EventName { get; set; }
}