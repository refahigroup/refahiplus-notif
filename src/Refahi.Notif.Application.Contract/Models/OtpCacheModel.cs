namespace Refahi.Notif.Application.Contract.Models
{
    public class OtpCacheModel
    {
        public string Destination { get; set; }
        public string Code { get; set; }
        public string Type { get; set; } // sms or email
        public string? Purpose { get; set; }
        public int Attempts { get; set; } = 0;
        public int MaxAttempts { get; set; } = 3;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
