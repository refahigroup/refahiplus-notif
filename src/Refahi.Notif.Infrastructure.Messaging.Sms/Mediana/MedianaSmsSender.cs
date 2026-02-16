using Microsoft.Extensions.Logging;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter.Enums;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Refahi.Notif.Infrastructure.Messaging.Sms.Mediana;

public class MedianaSmsSender : ISmsSender
{
    private readonly MedianaSmsConfiguration _config;
    private readonly HttpClient _client;
    private readonly ILogger<MedianaSmsSender> _logger;

    private const string SendSmsEndpoint = "/sms/v1/send/sms";
    private const string SendPatternEndpoint = "/sms/v1/send/pattern";

    public SmsGateway Gateway => SmsGateway.Mediana;

    public MedianaSmsSender(MedianaSmsConfiguration config, HttpClient client, ILogger<MedianaSmsSender> logger)
    {
        _config = config;
        _client = client;
        _logger = logger;
    }


    private static string NormalizePhoneNumber(string phoneNumber)
    {
        // Remove any non-digit characters
        var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());

        // Handle different formats:
        // 09XXXXXXXXX -> 989XXXXXXXXX
        // 9XXXXXXXXX -> 989XXXXXXXXX
        // 00989XXXXXXXXX -> 989XXXXXXXXX
        // +989XXXXXXXXX -> 989XXXXXXXXX
        // 989XXXXXXXXX -> 989XXXXXXXXX
        
        if (digits.StartsWith("00"))
        {
            digits = digits.Substring(2);
        }
        
        if (digits.StartsWith("0"))
        {
            return "98" + digits.Substring(1);
        }
        else if (digits.StartsWith("98"))
        {
            return digits;
        }
        else
        {
            return "98" + digits;
        }
    }
    private string? GetPatternCode(string templateName)
    {
        // TODO: Map template names to actual pattern codes from Mediana panel
        // For now, return null to use fallback simple SMS
        return null;
    }
    private string BuildTemplateMessage(string templateName, Dictionary<string, string> parameters)
    {
        return templateName switch
        {
            "VerificationCode" or "verification-code" => 
                SmsTemplates.VerificationCode(
                    parameters.GetValueOrDefault("code", "N/A"),
                    int.TryParse(parameters.GetValueOrDefault("validMinutes", "5"), out var mins) ? mins : 5
                ),
            
            "Welcome" or "welcome" => 
                SmsTemplates.Welcome(parameters.GetValueOrDefault("username", "کاربر گرامی")),
            
            "PasswordReset" or "password-reset" => 
                SmsTemplates.PasswordReset(parameters.GetValueOrDefault("code", "N/A")),
            
            "SecurityAlert" or "security-alert" => 
                SmsTemplates.SecurityAlert(parameters.GetValueOrDefault("deviceInfo", "دستگاه ناشناس")),
            
            "Notification" or "notification" => 
                SmsTemplates.Notification(
                    parameters.GetValueOrDefault("title", "اعلان"),
                    parameters.GetValueOrDefault("message", "")
                ),
            
            _ => string.Join("\n", parameters.Select(p => $"{p.Key}: {p.Value}"))
        };
    }
    private Dictionary<string, string> GetHttpHeaders()
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("X-API-KEY", _config.ApiKey);
        headers.Add("Accept", "application/json");

        return headers;
    }

    public async Task<(string, bool)> SendAsync(string[] phoneNumbers, string message, string? sender)
    {
        string msg = "";

        if (phoneNumbers.Length == 0)
            msg = "No phone numbers provided for SMS";
        else if (phoneNumbers.Length > 100)
            msg = $"Maximum 100 recipients allowed per SMS. Provided: {phoneNumbers.Length}";


        if(!string.IsNullOrEmpty(msg))
        {
            _logger.LogWarning(msg);
            return (msg, false);
        }


        var finalMessage = message.Contains("لغو11") ? message : $"{message}\n\nلغو11";

        var url = new Uri(new Uri(_config.BaseUrl), SendSmsEndpoint);

        var data = new
        {
            Type = _config.MessageType,
            Recipients = phoneNumbers.Select(NormalizePhoneNumber).ToArray(),
            MessageText = finalMessage
        };

        var result = await _client.PostString(url.ToString(), data, GetHttpHeaders());

        MedianaSmsResponse response;

        try
        {
            response = result.DeSerilize<MedianaSmsResponse>();

            if (response == null || response.Data == null || !response.Data.Succeed)
                throw new Exception("Response Not Valid");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in Deserilize Sms Result");
            return (string.Empty, true);
        }
        //if (response.Code == 400)
        //{
        //    _logger.LogError($"Bad request: ${string.Join(",", phoneNumbers)}");
        //    return (string.Empty, true);
        //}
        //if (response.Code == 401)
        //{
        //    _logger.LogError($"Permission Denied: ${string.Join(",", phoneNumbers)}");
        //    return (string.Empty, true);
        //}
        //if (response.Code == 403)
        //{
        //    _logger.LogError($"Forbiden: ${string.Join(",", phoneNumbers)}");
        //    return (string.Empty, true);
        //}

        //if (response.Code != 200)
        //    throw new Exception($"Error In Send Sms : {response.Code} - {response.Status}");

        return (response.Data.RequestId.ToString(), false);
    }
    public async Task<string> VerifyAsync(VerifySmsTemplate template, string phoneNumber, string code1, string? code2 = null, string? code3 = null, bool needTag = true)
    {
        string templateName = "VerificationCode";

        try
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("code", code1);

            var data = new MedianaSendPatternRequest
            {
                Type = _config.MessageType,
                Recipients = new[] { NormalizePhoneNumber(phoneNumber) },
                PatternCode = templateName,
                Parameters = parameters
            };

            var url = new Uri(new Uri(_config.BaseUrl), SendPatternEndpoint);

            var result = await _client.PostString(url.ToString(), data, GetHttpHeaders());

            MedianaReponse<MedianaSmsResponse> response;

            try
            {
                response = result.DeSerilize<MedianaReponse<MedianaSmsResponse>>();

                if (response == null || response.Data == null || response.Data.Data == null || !response.Data.Data.Succeed)
                    throw new Exception("Response Not Valid");

                return string.Empty;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while sending pattern SMS, falling back to simple SMS");

                var message = BuildTemplateMessage(templateName, parameters);

                var r = await SendAsync(new[] { phoneNumber }, message, null);

                return r.Item1;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception while sending pattern SMS: {ex.Message}");
        }

        return string.Empty;
    }
    public Task<string> VerifyAudioAsync(VerifySmsTemplate template, string phoneNumber, string code1, string? code2 = null, string? code3 = null)
    {
        throw new NotImplementedException();
    }
}
