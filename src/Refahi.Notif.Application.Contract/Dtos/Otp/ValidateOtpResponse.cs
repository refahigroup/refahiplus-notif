namespace Refahi.Notif.Application.Contract.Dtos.Otp
{
    public class ValidateOtpResponse
    {
        public bool IsValid { get; set; }
        public string Destination { get; set; } = string.Empty;
        public string DestinationType { get; set; } = string.Empty;
        public int AttemptsRemaining { get; set; }
        public string Message { get; set; }
    }
}
