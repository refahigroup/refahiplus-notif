namespace Refahi.Notif.Infrastructure.Messaging.PushNotification.APN
{
    public class APNConfiguration
    {
        public string BundleId { get; set; }
        public string KeyId { get; set; }
        public string TeamId { get; set; }
        public string CertFilePath { get; set; }
        public bool UseSandBox { get; set; }
        public string ProxyUrl { get; set; }
        public string ProxySecret { get; set; }
        public bool UseProxy { get; set; }
    }
}
