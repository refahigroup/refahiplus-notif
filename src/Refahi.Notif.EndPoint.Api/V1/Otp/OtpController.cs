using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Refahi.Notif.Application.Contract.Dtos.Otp;

namespace Refahi.Notif.EndPoint.Api.V1.Otp
{
    /// <summary>
    /// OTP (One-Time Password) Management
    /// </summary>
    [ApiController]
    [Route("V1/[controller]")]
    public class OtpController : BaseController
    {
        private readonly IMediator _mediator;

        public OtpController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Generate and send OTP to destination (phone/email)
        /// </summary>
        /// <param name="request">OTP generation parameters</param>
        /// <returns>Reference code and expiration time</returns>
        [HttpPost("Generate")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenerateOtpResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<GenerateOtpResponse>> Generate([FromBody] GenerateOtpRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Validate OTP code
        /// </summary>
        /// <param name="request">Reference code and OTP code</param>
        /// <returns>Validation result with attempts remaining</returns>
        [HttpPost("Validate")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ValidateOtpResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ValidateOtpResponse>> Validate([FromBody] ValidateOtpRequest request)
        {
            var response = await _mediator.Send(request);
            
            // Return 400 Bad Request for invalid OTP
            if (!response.IsValid)
                return BadRequest(response);
            
            return Ok(response);
        }
    }
}
