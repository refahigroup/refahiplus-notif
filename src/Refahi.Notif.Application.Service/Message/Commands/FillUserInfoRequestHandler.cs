using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refahi.Notif.Application.Contract.Dtos.User.Commands;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Utility;

namespace Refahi.Notif.Application.Service.Message.Commands
{
    public class FillUserInfoRequestHandler : IRequestHandler<FillUserInfoRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        readonly ILogger<FillUserInfoRequestHandler> _logger;
        private readonly string _identityApiUrl;

        public FillUserInfoRequestHandler(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<FillUserInfoRequestHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _identityApiUrl = configuration.GetValue<string>("Identity:IdentityApi") ?? "";
        }
        public async Task Handle(FillUserInfoRequest request, CancellationToken cancellationToken)
        {
            var userData = await GetUserInfo(request.UserId);
            if (userData == null)
                return;
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(request.UserId);
                if (user == null)
                {
                    user = new Domain.Core.Aggregates.UserAgg.User(userData.User.Id);
                    user.SetEmail(userData.User.Email);
                    user.SetPhoneNumber(userData.User.PhoneNumber);
                    await _unitOfWork.UserRepository.AddAsync(user);
                }
                else
                {
                    user.SetEmail(userData.User.Email);
                    user.SetPhoneNumber(userData.User.PhoneNumber);
                    _unitOfWork.UserRepository.Update(user);
                }

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In FillUserInfoRequest: {UserId},Error:{Error}", request.Serilize(), ex.Message);
            }

            return;
        }
        private async Task<UserData?> GetUserInfo(long userId)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_identityApiUrl}/API/Users/{userId}");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserData>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In GetUserInfo: {UserId},Error:{Error}", userId, ex.Message);
                return null;
            }
        }
    }

    public partial class UserData
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("userInfo")]
        public UserInfo UserInfo { get; set; }
    }

    public partial class User
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("emailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("phoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [JsonProperty("lockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [JsonProperty("twoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }

        [JsonProperty("accessFailedCount")]
        public long? AccessFailedCount { get; set; }

        [JsonProperty("lockoutEnd")]
        public DateTimeOffset? LockoutEnd { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class UserInfo
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("gender")]
        public object Gender { get; set; }

        [JsonProperty("nezamCode")]
        public string NezamCode { get; set; }

        [JsonProperty("birthDate")]
        public object BirthDate { get; set; }

        [JsonProperty("utmInfo")]
        public object UtmInfo { get; set; }
    }
}
