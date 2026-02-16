using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refahi.Notif.Domain.Contract;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Infrastructure.Messaging.Sms;
using Refahi.Notif.Messages.NotifCenter.Enums;
using System.Net.Http.Headers;
using System.Text;
using static Refahi.Notif.Domain.Core.Utility.HttpCall;

namespace Refahi.Notif.Infrastructure.Messaging.Sms.MedianaSMSHub
{
    public class MedianaSmsHubSender : ISmsSender
    {
        private readonly MedianaHubSmsConfiguration _medianaSmsConfig;
        //private readonly IDistributedCache _cache;
        private readonly ICacheService _cacheService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MedianaSmsHubSender> _logger;
        private readonly SmsTemplate _smsConfig;

        public MedianaSmsHubSender(
            IOptions<MedianaHubSmsConfiguration> medianaSmsConfig, 
            ILogger<MedianaSmsHubSender> logger,
            SmsTemplate smsComfig, 
            IHttpClientFactory httpClientFactory, 
            //IDistributedCache cache,
            ICacheService cacheService
        )
        {
            _medianaSmsConfig = medianaSmsConfig.Value;
            _logger = logger;
            _smsConfig = smsComfig;
            _httpClientFactory = httpClientFactory;
            //_cache = cache;
            _cacheService = cacheService;
        }

        public SmsGateway Gateway => SmsGateway.MedianaHub;


        private async Task<string> GetToken()
        {
            //var cachedTokenByteArray = await _cache.GetAsync("MedianaToken");
            //var cachedTokenObject = cachedTokenByteArray?.FromByteArray<MedianaHubTokenResponse>() ?? null;
            //if (cachedTokenObject is not null)
            //    return cachedTokenObject.AccessToken;

            var cachedTokenByteArray = await _cacheService.GetAsync<MedianaHubTokenResponse>("MedianaToken");

            if(cachedTokenByteArray  != null)
                return cachedTokenByteArray.AccessToken;


            var client = _httpClientFactory.CreateClient("MedianaSmsHub");

            var data = new List<KeyValuePair<string, string>>
            {
                new("Scope" , _medianaSmsConfig.Scope),
                new("Username" , _medianaSmsConfig.Username),
                new("Password", _medianaSmsConfig.Password)
            };

            var callApiRequest = new CallApiRequest()
            {
                Action = _medianaSmsConfig.GetTokenAction,
                MethodType = HttpMethod.Post,
                RequestContent = new FormUrlEncodedContent(data)
            };

            var result = await client.SendAsync<MedianaHubTokenResponse>(callApiRequest, c => !c.DeserializationSucceed || !c.RequestSucceed, 3);

            if (result is { RequestSucceed: false, DeserializationSucceed: true }) 
                return string.Empty;

            if (result.Data == null)
            {
                Thread.Sleep(1000);
                result = await client.SendAsync<MedianaHubTokenResponse>(callApiRequest, c => !c.DeserializationSucceed || !c.RequestSucceed, 3);
            }

            _logger.LogError("GetToken : {@result}", result.Data.ToJson());
            _logger.LogError("GetToken : {@result}", DateTimeOffset.Now.AddMinutes(5));

            //await _cacheS.SetAsync("MedianaToken", result.Data.ToByteArray(), new DistributedCacheEntryOptions()
            //{
            //    AbsoluteExpiration = result.Data.Expires
            //});

            await _cacheService.SetAsync("MedianaToken", result.Data.ToByteArray(), result.Data.Expires);

            return result.Data.AccessToken;
        }

        public async Task<(string, bool)> SendAsync(string[] phoneNumbers, string message, string? sender)
        {
            var client = _httpClientFactory.CreateClient("MedianaSmsHub");
            var token = await GetToken();

            if (string.IsNullOrEmpty(token))
                return (null, true);


            client.DefaultRequestHeaders.Authorization = new
                AuthenticationHeaderValue("Bearer", token);

            var data = new MedianaHubSendSmsRequest()
            {
                DestinationAddress = phoneNumbers.Select(c => c.NormalizePhoneNumberWithCountryCode()).ToArray(),
                MessageText = message,
                SourceAddress = _medianaSmsConfig.Sender,
                ValidityPeriod = DateTime.Now.AddHours(2)
            };

            var request = new CallApiRequest()
            {
                Action = _medianaSmsConfig.SendSmsAction,
                MethodType = HttpMethod.Post,
                RequestContent = new StringContent(data.ToJson(), Encoding.UTF8, "application/json")
            };

            var result = await client.SendAsync<MedianaHubSendSmsResponse>(request);

            if (!result.DeserializationSucceed || !result.RequestSucceed)
            {
                _logger.LogError("Error in sending message by Mediana hub,exception: {@exception}", result.Exception.Message);
                return (null, true);

            }

            if (result.Data.Succeeded) return (result.Data.IdList.FirstOrDefault(), false)!;

            _logger.LogError("Error in sending message by Mediana hub,result: {@result}", result.Data.ToJson());
            return (null, true);


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
