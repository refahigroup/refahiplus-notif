using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;
using Refahi.Notif.Infrastructure.Messaging.PushNotification.Firebase.Model;
using Refahi.Notif.Infrastructure.Messaging.PushNotification.Utility;
using System.Net;
using System.Security.Cryptography;
using System.Text;


namespace Refahi.Notif.Infrastructure.Messaging.PushNotification.Firebase
{
    public class FirebaseNotificationSender : IPushNotificationSender
    {
        private static string CacheKey = "FirebaseToken";
        private readonly FirebaseConfiguration _config;
        private readonly HttpClient _client;
        private readonly ILogger<FirebaseNotificationSender> _logger;
        private readonly IDistributedCache _cache;
        private readonly FirebaseSettings _settings;
        public FirebaseNotificationSender(
            FirebaseConfiguration config,
            ILogger<FirebaseNotificationSender> logger,
            HttpClient client,
            IDistributedCache cache)
        {
            _config = config;
            _client = client;
            _logger = logger;
            _cache = cache;
            var tokenText = File.ReadAllText(config.TokenFilePath);
            _settings = JsonConvert.DeserializeObject<FirebaseSettings>(tokenText);
        }
        //todo : save result
        public async Task<MessageSendingResult> Send(string address, string title, string body, string url, string data, Guid messageMessageId)
        {
            var messageData = new Dictionary<string, object>();
            messageData.Add("MessageId", messageMessageId.ToString());
            var res = new MessageSendingResult();
            try
            {
                res = await SendAsync(new Message()
                {
                    Token = address,
                    Webpush = new WebpushConfig()
                    {
                        Notification = new WebpushNotification()
                        {
                            Title = title,
                            Body = body,
                            Icon = "https://refahiplus.com/logo.png",
                            RequireInteraction = true,
                            Silent = false,
                            Data = data,
                            CustomData = messageData
                        },
                        FcmOptions = new WebpushFcmOptions()
                        {
                            Link = url
                        }
                    },
                }, CancellationToken.None);
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return res;
        }
        public async Task<MessageSendingResult> SendAsync(Message message, CancellationToken cancellationToken = default)
        {
            var json = JsonConvert.SerializeObject(new { message }, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            using var HttpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_config.FCMUrlPath}/v1/projects/{_settings.ProjectId}/messages:send");

            var token = await GetJwtTokenAsync();

            HttpRequestMessage.Headers.Add("Authorization", $"Bearer {token}");
            HttpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await _client.SendAsync(HttpRequestMessage, cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    var error = JsonConvert.DeserializeObject<FirebaseErrorResponse>(responseString);
                    if (error != null && error.Error != null && error.Error.Details.Any())
                        if (error.Error.Details.Any(p => p.ErrorCode == "UNREGISTERED"))
                            return new MessageSendingResult()
                            {
                                Message = error.Error.Message,
                                Success = false,
                                NotificationToken = message.Token,
                                ErrorType = MessageSendingErrorType.InvalidAddress
                            };
                }
                throw new HttpRequestException("Firebase notification error: " + responseString);
            }
            var r = JsonConvert.DeserializeObject<FirebaseResponse>(responseString);
            return new MessageSendingResult() { Message = r.Name, Success = true };
        }
        private async Task<string> GetJwtTokenAsync()
        {
            var accessToken = await _cache.GetStringAsync(CacheKey);
            if (!string.IsNullOrEmpty(accessToken))
            {
                return accessToken;
            }

            using var message = new HttpRequestMessage(HttpMethod.Post, $"{_config.OAuth2Url}/token");
            using var form = new MultipartFormDataContent();
            var authToken = GetMasterToken();
            form.Add(new StringContent(authToken), "assertion");
            form.Add(new StringContent("urn:ietf:params:oauth:grant-type:jwt-bearer"), "grant_type");
            message.Content = form;

            using var response = await _client.SendAsync(message);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Firebase error when creating JWT token: " + content);
            }

            var firebaseToken = JsonConvert.DeserializeObject<FirebaseTokenResponse>(content);
            var firebaseTokenExpiration = DateTime.UtcNow.AddSeconds(firebaseToken.ExpiresIn - 10);
            if (string.IsNullOrWhiteSpace(firebaseToken.AccessToken) || firebaseTokenExpiration < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Couldn't deserialize firebase token response");
            }
            await _cache.SetStringAsync(CacheKey, firebaseToken.AccessToken, new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = firebaseTokenExpiration
            });
            return firebaseToken.AccessToken;
        }
        private string GetMasterToken()
        {
            var header = JsonConvert.SerializeObject(new { alg = "RS256", typ = "JWT" });
            var payload = JsonConvert.SerializeObject(new
            {
                iss = _settings.ClientEmail,
                aud = _settings.TokenUri,
                scope = "https://www.googleapis.com/auth/firebase.messaging",
                iat = CryptoHelper.GetEpochTimestamp(),
                exp = CryptoHelper.GetEpochTimestamp() + 3600 /* has to be short lived */
            });

            var headerBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(header));
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
            var unsignedJwtData = $"{headerBase64}.{payloadBase64}";
            var unsignedJwtBytes = Encoding.UTF8.GetBytes(unsignedJwtData);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(_settings.PrivateKey.ToCharArray());

            var signature = rsa.SignData(unsignedJwtBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var signatureBase64 = Convert.ToBase64String(signature);

            return $"{unsignedJwtData}.{signatureBase64}";
        }
    }


}
