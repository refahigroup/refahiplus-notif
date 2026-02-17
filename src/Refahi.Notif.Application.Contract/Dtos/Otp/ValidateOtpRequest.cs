using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Refahi.Notif.Application.Contract.Dtos.Otp
{
    public class ValidateOtpRequest : IRequest<ValidateOtpResponse>
    {
        /// <summary>
        /// Reference code returned from Generate endpoint
        /// </summary>
        [Required]
        public string ReferenceCode { get; set; }

        /// <summary>
        /// OTP code provided by user
        /// </summary>
        [Required]
        [StringLength(10, MinimumLength = 4)]
        public string Code { get; set; }
    }
}
