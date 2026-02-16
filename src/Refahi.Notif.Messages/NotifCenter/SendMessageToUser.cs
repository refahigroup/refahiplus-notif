using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Messages.NotifCenter
{

    public class SendMessageToUser
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public DateTime? DueTime { get; set; }
        public string? ValidatorUrl { get; set; }
        public string[]? Tags { get; set; }
        public AppName? AppName { get; set; }
        public SendSmsToUserRequest? Sms { get; set; }
        public SendEmailToUserRequest? Email { get; set; }
        public SendTelegramToUserRequest? Telegram { get; set; }
        public SendPushNotificationToUserRequest? PushNotification { get; set; }
        public SendNotificationToUserRequest? Notification { get; set; }
    }

    public class SendSmsToUserRequest
    {
        public string Body { get; set; }
        public string? Sender { get; set; }
        public SmsGateway? Gateway { get; set; }
    }
    public class SendEmailToUserRequest
    {

        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
    }
    public class SendTelegramToUserRequest
    {
        public string ChatId { get; set; }
        public string? Body { get; set; }
        public string? FileName { get; set; }
        public byte[]? FileData { get; set; }
    }

    public class SendPushNotificationToUserRequest
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Url { get; set; }
        public string Data { get; set; }
    }
    public class SendNotificationToUserRequest
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? Link { get; set; }
        public string? Icon { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}
