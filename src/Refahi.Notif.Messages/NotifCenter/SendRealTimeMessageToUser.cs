namespace Refahi.Notif.Messages.NotifCenter
{

    public class SendRealTimeMessageToUser
    {
        public long UserId { get; set; }
        public string Type { get; set; }
        public string Body { get; set; }
    }
}
