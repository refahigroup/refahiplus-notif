namespace Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;

public class MessageSendingResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string NotificationToken { get; set; }
    public MessageSendingErrorType ErrorType { get; set; }

    public MessageSendingResult()
    {
        ErrorType = MessageSendingErrorType.None;
    }
}

public enum MessageSendingErrorType
{
    None,
    InvalidAddress,
}