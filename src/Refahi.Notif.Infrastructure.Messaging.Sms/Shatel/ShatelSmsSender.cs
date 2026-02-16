using Microsoft.Extensions.Logging;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Infrastructure.Messaging.Sms;
using Refahi.Notif.Messages.NotifCenter.Enums;
using System.Web;

namespace Refahi.Notif.Infrastructure.Messaging.Sms.Shatel
{
    public class ShatelSmsSender : ISmsSender
    {
        private const int MsgEncodingAscii = 0;
        private const int MsgEncodingUtf8 = 8;
        private readonly ShatelSmsConfiguration _shatelSmsConfig;
        private readonly ILogger<ShatelSmsSender> _logger;
        private readonly SmsTemplate _smsConfig;
        private readonly HttpClient _client;

        public ShatelSmsSender(
            ShatelSmsConfiguration shatelSmsConfig,
            HttpClient client,
            ILogger<ShatelSmsSender> logger,
            SmsTemplate smsComfig
        )
        {
            _shatelSmsConfig = shatelSmsConfig;
            _logger = logger;
            _smsConfig = smsComfig;
            _client = client;
        }

        public SmsGateway Gateway => SmsGateway.Shatel;

        public async Task<(string, bool)> SendAsync(string[] phoneNumbers, string message, string? sender)
        {
            message = HttpUtility.UrlEncode(message);
            var url = $"https://panel.shatelmobile.ir/webservice/v1rest/sendsms?username={_shatelSmsConfig.UserName}&password={HttpUtility.UrlEncode(_shatelSmsConfig.Password)}&source={_shatelSmsConfig.Sender}&destination={string.Join(",", phoneNumbers)}&message={message}";

            var result = await _client.GetString(url, null, false);
            if (result == null || string.IsNullOrEmpty(result))
            {
                throw new Exception("Response Null");
            }
            if (!result.Contains("["))
            {
                throw new Exception("Response is invalid: " + result);
            }
            return (result.Substring(1, result.Length - 2), false)!;
        }

        public async Task<string> VerifyAsync(VerifySmsTemplate template, string phoneNumber, string code1,
            string? code2 = null, string? code3 = null, bool needTag = true)
        {
            var message = _smsConfig.GetVerifyMessage(template, code1, code2, needTag);
            return (await SendAsync(new string[] { phoneNumber }, message, null)).Item1;
        }

        public Task<string> VerifyAudioAsync(VerifySmsTemplate template, string phoneNumber, string code1, string? code2 = null, string? code3 = null)
        {
            throw new NotImplementedException();
        }


    }
}
