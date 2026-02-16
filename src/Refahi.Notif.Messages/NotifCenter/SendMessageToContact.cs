using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Messages.NotifCenter
{

    public class SendMessageToContact
    {
        public Guid Id { get; set; }
        public long? UserId { get; set; }
        public long? Owner { get; set; }
        public DateTime? DueTime { get; set; }
        public string[]? PhoneNumbers { get; set; }
        public string? ValidatorUrl { get; set; }
        public string[]? Tags { get; set; }
        public AppName? AppName { get; set; }
        public SendSmsToContactRequest? Sms { get; set; }
        public SendPushNotificationToContactRequest? PushNotification { get; set; }
        public SendEmailToContactRequest? Email { get; set; }
        public SendNotificationToContactRequest? Notification { get; set; }
        public SendTelegramMessageToContactRequest? TelegramMessage { get; set; }
        public NotificationStep[]? Steps { get; set; }
    }

    public class SendSmsToContactRequest
    {
        public string Body { get; set; }
        public string? Sender { get; set; }
        public DateTime? DueTime { get; set; }
        public SmsGateway? Gateway { get; set; }
    }
    public class SendEmailToContactRequest
    {

        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
    }
    public class SendPushNotificationToContactRequest
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Url { get; set; }
        public string Data { get; set; }
        public DateTime? DueTime { get; set; }
    }
    public class SendNotificationToContactRequest
    {
        public DateTime? ExpiredDate { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? Link { get; set; }
        public string? Icon { get; set; }
        public DateTime? DueTime { get; set; }
    }
    public class SendTelegramMessageToContactRequest
    {
        public string ChatId { get; set; }
        public string? Body { get; set; }
        public string? FileName { get; set; }
        public byte[]? FileData { get; set; }
        public DateTime? DueTime { get; set; }
    }
    public enum AppName
    {
        PHR = 0,
        EMR = 1,
        PHRReminder = 2,
    }
    public enum NotificationType
    {
        General = 1,
        Wallet = 2,
        Order = 3,
        Visit = 4
    }
    public class NotificationStep
    {
        public NotificationChannelType NotificationChannelType { get; set; }
        public TimeSpan? DelayAfterPreviousStep { get; set; }
        public bool? SkipIfAnyPreviousStepHandled { get; set; }
    }
    public enum NotificationChannelType
    {
        Sms,
        Email,
        PushNotification,
        Notification,
        TelegramMessage
    }
}
