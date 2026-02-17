using MediatR;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.Otp;
using Refahi.Notif.Application.Contract.Models;
using Refahi.Notif.Domain.Contract;

namespace Refahi.Notif.Application.Service.Otp
{
    public class ValidateOtpHandler : IRequestHandler<ValidateOtpRequest, ValidateOtpResponse>
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<ValidateOtpHandler> _logger;

        public ValidateOtpHandler(
            ICacheService cacheService,
            ILogger<ValidateOtpHandler> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<ValidateOtpResponse> Handle(ValidateOtpRequest request, CancellationToken cancellationToken)
        {
            var cacheKey = GetCacheKey(request.ReferenceCode);

            // Get OTP from cache
            var cachedOtp = await _cacheService.GetAsync<OtpCacheModel>(cacheKey);

            if (cachedOtp == null)
            {
                _logger.LogWarning("OTP validation failed: Reference code not found or expired {ReferenceCode}", 
                    request.ReferenceCode);
                
                return new ValidateOtpResponse
                {
                    IsValid = false,
                    AttemptsRemaining = 0,
                    Message = "OTP expired or invalid reference code"
                };
            }

            // Check if expired
            if (cachedOtp.ExpiresAt < DateTime.Now)
            {
                _logger.LogWarning("OTP validation failed: Expired {ReferenceCode}", request.ReferenceCode);
                
                return new ValidateOtpResponse
                {
                    IsValid = false,
                    AttemptsRemaining = 0,
                    Message = "OTP has expired"
                };
            }

            // Check max attempts
            if (cachedOtp.Attempts >= cachedOtp.MaxAttempts)
            {
                _logger.LogWarning("OTP validation failed: Max attempts exceeded {ReferenceCode}", 
                    request.ReferenceCode);
                
                return new ValidateOtpResponse
                {
                    IsValid = false,
                    AttemptsRemaining = 0,
                    Message = "Maximum attempts exceeded"
                };
            }

            // Validate code
            if (cachedOtp.Code == request.Code)
            {
                // Valid - remove from cache
                // Note: Redis doesn't have a direct delete in ICacheService, 
                // but we can set it with expired time
                await _cacheService.SetAsync(cacheKey, cachedOtp, DateTimeOffset.UtcNow.AddSeconds(-1));

                _logger.LogInformation("OTP validated successfully for {ReferenceCode}", request.ReferenceCode);

                return new ValidateOtpResponse
                {
                    IsValid = true,
                    AttemptsRemaining = 0,
                    Message = "OTP verified successfully"
                };
            }

            // Invalid code - increment attempts
            cachedOtp.Attempts++;
            var attemptsRemaining = cachedOtp.MaxAttempts - cachedOtp.Attempts;

            // Update cache with new attempts count
            await _cacheService.SetAsync(cacheKey, cachedOtp, cachedOtp.ExpiresAt);

            _logger.LogWarning("OTP validation failed: Invalid code {ReferenceCode}, attempts: {Attempts}/{MaxAttempts}", 
                request.ReferenceCode, cachedOtp.Attempts, cachedOtp.MaxAttempts);

            return new ValidateOtpResponse
            {
                IsValid = false,
                AttemptsRemaining = attemptsRemaining,
                Message = $"Invalid OTP code. {attemptsRemaining} attempts remaining"
            };
        }

        private static string GetCacheKey(string referenceCode)
        {
            return $"otp:{referenceCode}";
        }
    }
}
