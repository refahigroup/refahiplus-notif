namespace Refahi.Notif.Messages.NotifCenter
{
    public class SendSmsMessage
    {
        public Guid MessageId { get; set; }
    }

    public class SendEmailMessage
    {
        public Guid MessageId { get; set; }
    }

    public class SendPushMessage
    {
        public Guid MessageId { get; set; }
        //public string[] Addresses { get; set; }
    }
    public class SendNotificationMessage
    {
        public Guid MessageId { get; set; }
        //public string[] Addresses { get; set; }
    }
    public class SendTelegramMessage
    {
        public Guid MessageId { get; set; }
    }


    public class SendRealTimeMessages
    {
        public Guid MessageId { get; set; }
    }
}
