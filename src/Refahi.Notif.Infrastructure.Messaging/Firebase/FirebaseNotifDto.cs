namespace Refahi.Notif.Infrastructure.Messaging.PushNotification.Firebase
{
    public class FirebaseNotifDto
    {
        public string[] registration_ids { get; set; }
        public FirebaseNotifMessageDto notification { get; set; }
        //public FirebaseNotifDataDto data { get; set; }
        public dynamic data { get; set; }
    }
    public class FirebaseNotifMessageDto
    {
        public string title { get; set; }
        public string body { get; set; }
        public string click_action { get; set; }
    }
    public class FirebaseNotifDataDto
    {
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
    }
}
