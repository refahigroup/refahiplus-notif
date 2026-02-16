using Refahi.Notif.Domain.Core.Utility;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Refahi.Notif.Infrastructure.Messaging.Sms.KaveNegar
{
    public class KaveSmsCreditChecker
    {
        private readonly HttpClient _httpClient;
        private readonly KaveSmsConfiguration _config;

        public KaveSmsCreditChecker(HttpClient httpClient, KaveSmsConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<HealthCheckResult> Check()
        {
            //var url = $"http://api.kavenegar.com/v1/{_config.Token}/account/info.json";

            //var result = await _httpClient.Get<SendSmsResultModel<GetInfoResultModel>>(url);

            //var desc = $"Remaining Credit Is : {result.Entries.RemainCredit}";

            //if (result.Entries.RemainCredit < _config.MinimumCredit)
            //    return HealthCheckResult.Unhealthy(desc);

            //return HealthCheckResult.Healthy(desc);

            return new HealthCheckResult(HealthStatus.Healthy);
        }

    }
}
