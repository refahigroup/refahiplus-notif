using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refahi.Notif.Application.Contract.Configuration;
using Refahi.Notif.Application.Contract.Dtos.Otp;
using Refahi.Notif.Application.Contract.Models;
using Refahi.Notif.Domain.Contract;
using Refahi.Notif.Messages.NotifCenter;
using Refahi.Notif.Messages.NotifCenter.Enums;
using System.Security.Cryptography;

namespace Refahi.Notif.Application.Service.Otp
{
    public class GenerateOtpHandler : IRequestHandler<GenerateOtpRequest, GenerateOtpResponse>
    {
        private readonly ICacheService _cacheService;
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly ILogger<GenerateOtpHandler> _logger;
        private readonly OtpConfiguration _otpConfig;

        public GenerateOtpHandler(
            ICacheService cacheService,
            IBus bus,
            IMapper mapper,
            ILogger<GenerateOtpHandler> logger,
            IOptions<OtpConfiguration> otpConfig)
        {
            _cacheService = cacheService;
            _bus = bus;
            _mapper = mapper;
            _logger = logger;
            _otpConfig = otpConfig.Value;
        }

        public async Task<GenerateOtpResponse> Handle(GenerateOtpRequest request, CancellationToken cancellationToken)
        {
            // Use configured defaults if not provided
            var length = request.Length ?? _otpConfig.DefaultLength;
            var ttlMinutes = request.TtlMinutes ?? _otpConfig.DefaultTtlMinutes;

            // Generate OTP code
            var otpCode = GenerateOtpCode(length);

            // Generate unique reference code
            var referenceCode = GenerateReferenceCode();

            // Calculate expiration
            var expiresAt = DateTime.Now.AddMinutes(ttlMinutes);

            // Create cache model
            var cacheModel = new OtpCacheModel
            {
                Destination = request.Destination,
                Code = otpCode,
                Type = request.Type.ToLower(),
                Purpose = request.Purpose,
                Attempts = 0,
                MaxAttempts = _otpConfig.MaxAttempts,
                CreatedAt = DateTime.Now,
                ExpiresAt = expiresAt
            };

            // Store in Redis
            var cacheKey = GetCacheKey(referenceCode);
            await _cacheService.SetAsync(cacheKey, cacheModel, expiresAt);

            _logger.LogInformation("OTP generated for {Destination} with reference {ReferenceCode}", 
                MaskDestination(request.Destination), referenceCode);

            // Send OTP via existing mechanism
            await SendOtpAsync(request.Type, request.Destination, otpCode, request.Purpose);

            return new GenerateOtpResponse
            {
                ReferenceCode = referenceCode,
                ExpiresAt = expiresAt,
                Message = $"OTP sent to {MaskDestination(request.Destination)}"
            };
        }

        private async Task SendOtpAsync(string type, string destination, string code, string? purpose)
        {
            if (type.ToLower() == "sms")
            {
                // Use existing SendVerifySms message
                var verifySms = new SendVerifySms
                {
                    PhoneNumber = destination,
                    Code = code,
                    Template = DetermineSmsTemplate(purpose),
                    ExpireTime = DateTime.Now.AddMinutes(5),
                    IsAudio = false,
                    IsResend = false,
                    NeedTag = true
                };

                await _bus.Publish(verifySms);
            }
            else if (type.ToLower() == "email")
            {
                // TODO: Implement email sending when needed
                _logger.LogWarning("Email OTP sending not yet implemented");
            }
        }

        private VerifySmsTemplate DetermineSmsTemplate(string? purpose)
        {
            return purpose?.ToLower() switch
            {
                "login" => VerifySmsTemplate.Login,
                "signup" or "register" => VerifySmsTemplate.Register,
                "reset-password" or "forget-password" => VerifySmsTemplate.ForgetPassword,
                _ => VerifySmsTemplate.Register
            };
        }

        private static string GenerateOtpCode(int length)
        {
            const string chars = "0123456789";
            var result = new char[length];
            
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }
            
            return new string(result);
        }

        private static string GenerateReferenceCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 16);
        }

        private static string GetCacheKey(string referenceCode)
        {
            return $"otp:{referenceCode}";
        }

        private static string MaskDestination(string destination)
        {
            if (destination.Length <= 4) return "****";
            
            if (destination.Contains("@"))
            {
                var parts = destination.Split('@');
                return $"{parts[0].Substring(0, 2)}***@{parts[1]}";
            }
            
            return $"{destination.Substring(0, 4)}***{destination.Substring(destination.Length - 2)}";
        }
    }
}
