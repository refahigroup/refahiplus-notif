namespace Refahi.Notif.Application.Contract.Dtos.Otp
{
    public class GenerateOtpResponse
    {
        public string ReferenceCode { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Message { get; set; } = "OTP sent successfully";
    }
}
