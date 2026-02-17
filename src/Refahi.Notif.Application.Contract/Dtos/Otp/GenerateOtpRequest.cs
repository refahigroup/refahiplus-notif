using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Refahi.Notif.Application.Contract.Dtos.Otp
{
    public class GenerateOtpRequest : IRequest<GenerateOtpResponse>
    {
        /// <summary>
        /// Phone number or email address
        /// </summary>
        [Required]
        public string Destination { get; set; }

        /// <summary>
        /// Type of OTP delivery: sms or email
        /// </summary>
        [Required]
        public string Type { get; set; } = "sms";

        /// <summary>
        /// Purpose of OTP: login, signup, reset-password, etc.
        /// </summary>
        public string? Purpose { get; set; }

        /// <summary>
        /// Time to live in minutes (optional, uses default from configuration if not provided)
        /// </summary>
        public int? TtlMinutes { get; set; }

        /// <summary>
        /// Length of OTP code (optional, uses default from configuration if not provided)
        /// </summary>
        public int? Length { get; set; }
    }
}
