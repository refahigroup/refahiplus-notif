namespace Refahi.Notif.Infrastructure.Messaging.PushNotification.Firebase
{
    public class FirebaseConfiguration
    {
        public string AuthorizeHeader { get; set; }
        public string Url { get; set; }
        public bool UseProxy { get; set; }
        public string ProxyUrl { get; set; }
        public string ProxySecret { get; set; }
        public string TokenFilePath { get; set; }
        public string FCMUrlPath { get; set; }
        public string OAuth2Url { get; set; }
    }
}
