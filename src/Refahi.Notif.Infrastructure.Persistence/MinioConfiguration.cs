namespace Refahi.Notif.Infrastructure.Persistence
{
    public class MinioConfiguration
    {
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Protocol { get; set; }
        public string BucketName { get; set; }
        public string DirectoryName { get; set; }
    }
}
