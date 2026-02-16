//using Microsoft.Extensions.Logging;
//using Refahi.Notif.Domain.Contract.Messaging;
//using Refahi.Notif.Domain.Core.Utility;
//using Refahi.Notif.Infrastructure.Messaging.Sms.Mediana.old.Responses;
//using Refahi.Notif.Messages.NotifCenter.Enums;

//namespace Refahi.Notif.Infrastructure.Messaging.Sms.Mediana.old
//{
//    public class MedianaSmsSender : ISmsSender
//    {
//        private readonly MedianaSmsConfiguration _medianaSmsConfig;
//        private readonly HttpClient _client;
//        private readonly ILogger<MedianaSmsSender> _logger;
//        private readonly SmsTemplate _smsConfig;

//        public MedianaSmsSender(
//            MedianaSmsConfiguration medianaSmsConfig,
//            HttpClient client,
//            ILogger<MedianaSmsSender> logger,
//            SmsTemplate smsComfig
//        )
//        {
//            _medianaSmsConfig = medianaSmsConfig;
//            _client = client;
//            _logger = logger;
//            _smsConfig = smsComfig;

//            _client.DefaultRequestHeaders.Add("apikey", _medianaSmsConfig.Token);
//        }

//        public SmsGateway Gateway => SmsGateway.Mediana;

//        public async Task<(string, bool)> SendAsync(string[] phoneNumbers, string message, string? sender)
//        {
//            /*
//                {
//                  "recipient": [
//                    "+989120000000"
//                  ],
//                  "sender": "+983000505",
//                  "time": "2023-03-21T09:12:50.824Z",
//                  "message": "ارتباط بسازید. مدیانا"
//                }
//            */


//            var url = new Uri(new Uri(_medianaSmsConfig.BaseUrl), "sms/send/webservice/single");

//            var data = new
//            {
//                Recipient = phoneNumbers,
//                _medianaSmsConfig.Sender,
//                Message = message
//            };



//            var request = new HttpRequestMessage(HttpMethod.Post, url.ToString());
//            request.Content = new StringContent(data.Serilize());
//            request.Headers.Add("apikey", _medianaSmsConfig.Token);

//            var result = await _client.PostString(url.ToString(), data);


//            MedianaReponse<SendSmsResponseData> response;

//            try
//            {
//                //این قسمت از بلوک بیرونه چون خطا در این حالات نباید باعث تکرار عملیات بشه
//                response = result.DeSerilize<MedianaReponse<SendSmsResponseData>>();

//                if (response.Data == null || response.Data.MessageId == 0)
//                    throw new Exception("Response Not Valid");

//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error in Deserilize Sms Result");
//                return (null, false);
//            }

//            //wrong number
//            if (response.Code == 400)
//            {
//                _logger.LogError($"Bad request: ${string.Join(",", phoneNumbers)}");
//                return (null, true);
//            }
//            if (response.Code == 401)
//            {
//                _logger.LogError($"Permission Denied: ${string.Join(",", phoneNumbers)}");
//                return (null, true);
//            }
//            if (response.Code == 403)
//            {
//                _logger.LogError($"Forbiden: ${string.Join(",", phoneNumbers)}");
//                return (null, true);
//            }

//            if (response.Code != 200)
//                throw new Exception($"Error In Send Sms : {response.Code} - {response.Status}");

//            return (response.Data.MessageId.ToString(), false);

//        }

//        public async Task<string> VerifyAsync(VerifySmsTemplate template, string phoneNumber, string code1,
//            string? code2 = null, string? code3 = null, bool needTag = true)
//        {
//            var message = _smsConfig.GetVerifyMessage(template, code1, code2, needTag);
//            return (await SendAsync(new string[] { phoneNumber }, message, null)).Item1;
//        }

//        public Task<string> VerifyAudioAsync(VerifySmsTemplate template, string phoneNumber, string code1, string? code2 = null, string? code3 = null)
//        {
//            throw new NotImplementedException();
//        }


//    }
//}
