namespace Refahi.Notif.Application.Contract.Dtos.Otp
{
    public class ValidateOtpResponse
    {
        public bool IsValid { get; set; }
        public int AttemptsRemaining { get; set; }
        public string Message { get; set; }
    }
}
