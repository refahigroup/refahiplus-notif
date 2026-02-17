namespace Refahi.Notif.Application.Contract.Configuration
{
    public class OtpConfiguration
    {
        /// <summary>
        /// Default OTP time to live in minutes
        /// </summary>
        public int DefaultTtlMinutes { get; set; } = 5;

        /// <summary>
        /// Default OTP code length
        /// </summary>
        public int DefaultLength { get; set; } = 6;

        /// <summary>
        /// Maximum validation attempts allowed
        /// </summary>
        public int MaxAttempts { get; set; } = 3;
    }
}
