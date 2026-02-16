using Microsoft.Extensions.Logging;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter.Enums;
using System.Text;
using System.Web;

namespace Refahi.Notif.Infrastructure.Messaging.Sms.KaveNegar
{
    public class KaveSmsSender : ISmsSender
    {
        private readonly KaveSmsConfiguration _smsConfig;
        private readonly HttpClient _client;
        private readonly ILogger<KaveSmsSender> _logger;

        public KaveSmsSender(
            KaveSmsConfiguration smsConfig,
            HttpClient client,
            ILogger<KaveSmsSender> logger
            )
        {
            _smsConfig = smsConfig;
            _client = client;
            _logger = logger;
        }

        public SmsGateway Gateway => SmsGateway.Kavenegar;

        public async Task<(string, bool)> SendAsync(string[] phoneNumbers, string message, string? sender)
        {
            message = HttpUtility.UrlEncode(message);
            var url = $"http://api.kavenegar.com/v1/{_smsConfig.Token}/sms/send.json?sender={(string.IsNullOrEmpty(sender) ? _smsConfig.Sender : sender)}&receptor={string.Join(",", phoneNumbers)}&message={message}";

            string result;

            result = await _client.GetString(url, null, false);

            SendSmsResultModel<List<SendSmsEntriesResultModel>> response;
            try
            {
                //این قسمت از بلوک بیرونه چون خطا در این حالات نباید باعث تکرار عملیات بشه
                response = result.DeSerilize<SendSmsResultModel<List<SendSmsEntriesResultModel>>>();
                if (response.Return == null && response.Entries?[0] == null)
                    throw new Exception("Response Not Valid");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Deserilize Sms Result");
                return (null, false);
            }

            //wrong number
            if (response.Return.Status == 411)
            {
                _logger.LogError($"Sms Reciever Not Valid : ${string.Join(",", phoneNumbers)}");
                return (null, true);
            }
            if (response.Return.Status != 200)
                throw new Exception($"Error In Send Sms : {response.Return.Status} - {response.Return.StatusText}");

            return (response.Entries[0].Messageid.ToString(), false);
        }


        public async Task<string> VerifyAsync(VerifySmsTemplate template, string phoneNumber, string code1,
            string? code2 = null, string? code3 = null, bool needTag = true)
        {
            var kaveTemplate = template.GetKaveSmsTemplate(false);
            return await SendVerifyMessage(phoneNumber, kaveTemplate, code1, code2, code3);
        }

        public async Task<string> VerifyAudioAsync(VerifySmsTemplate template, string phoneNumber, string code1, string? code2 = null, string? code3 = null)
        {
            var kaveTemplate = template.GetKaveSmsTemplate(true);
            return await SendVerifyMessage(phoneNumber, kaveTemplate, code1, code2, code3);
        }

        private async Task<string?> SendVerifyMessage(string phoneNumber, KaveSmsTemplate template, string code1, string? code2 = null, string? code3 = null)
        {
            string token = _smsConfig.Token;

            var postData = new List<KeyValuePair<string, string>>
                {
                    new ("receptor",phoneNumber),
                    new ("token",HttpUtility.UrlEncode(code1)),
                    new ("template",template.ToString())
                };

            if (code2 != null)
                postData.Add(new("token2", HttpUtility.UrlEncode(code2)));

            if (code3 != null)
                postData.Add(new("token3", HttpUtility.UrlEncode(code3)));


            var result = await _client.PostUrlEncoded($"http://api.kavenegar.com/v1/{token}/verify/lookup.json", postData);

            try
            {
                //این قسمت از بلوک بیرونه چون خطا در این حالات نباید باعث تکرار عملیات بشه
                var response = result.DeSerilize<SendSmsResultModel<List<SendSmsEntriesResultModel>>>();

                //wrong number
                if (response.Return.Status == 411)
                {
                    _logger.LogError($"Sms Reciever Not Valid : ${phoneNumber}");
                    return null;
                }


                if (response.Return.Status != 200)
                    throw new Exception($"Error In Send Sms : {response.Return.StatusText}");

                return response.Entries[0].Messageid.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Deserilize Sms Result");
                return null;
            }


        }
    }



}
