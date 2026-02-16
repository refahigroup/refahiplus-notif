using Hangfire;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NikSms.Library.NetCore.WebApi;

namespace Refahi.Notif.Infrastructure.Messaging.Sms.Nik
{
    public class NikSmsUptimeChecker
    {

        public const int SuccessInterval = 5;
        public const int FailInterval = 1;
        private readonly HttpClient _httpClient;
        private readonly NikSmsConfiguration _config;
        private readonly PublicApiV1 _nikService;

        public static bool NikSmsIsUp { get; private set; } = true;
        public static string JobName { get; set; }

        public NikSmsUptimeChecker(HttpClient httpClient, NikSmsConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _nikService = new PublicApiV1(_config.UserName, _config.Password, "fa");
        }

        public async Task<HealthCheckResult> Check()
        {
            var interval = SuccessInterval;
            try
            {
                var data = await _nikService.GetServertime();
                if (data.Status != NikSms.Library.NetCore.ViewModels.LibOperationResultStatus.Success)
                    throw new Exception("NikSms Error");

                NikSmsIsUp = true;
                return HealthCheckResult.Healthy();

            }
            catch
            {
                NikSmsIsUp = false;
                interval = FailInterval;
                return HealthCheckResult.Unhealthy();
            }
            finally
            {
                Start(interval);
            }


        }

        public void Start(int interval = SuccessInterval)
        {
            if (JobName != null)
                BackgroundJob.Delete(JobName);

            JobName = BackgroundJob.Schedule(
                    () => Check(),
                    TimeSpan.FromMinutes(interval));
        }
    }
}
