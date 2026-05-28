using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Infrastructure.Messaging.Sms.KaveNegar;

namespace Refahi.Notif.EndPoint.Api.Startup
{
    public static class HealthCheck
    {
        private static string KaveNegarCheckName = "KaveNegar";
        private static string NikSmsCheckName = "NikSms";

        public static IHealthChecksBuilder AddHealthCheck(this IServiceCollection services, IConfiguration config)
        {
            return services.AddHealthChecks()
                .AddNpgSql((config["ConnectionStrings:PostgresNotif"] ?? "").ReplaceWithEnvironmentVariables())

                .AddCheck<KaveSmsCreditChecker>(KaveNegarCheckName)

                .AddHangfire(x =>
                {
                    x.MinimumAvailableServers = 1;
                });
                
                //.AddMassTransitBusHealthCheck();
                //.AddMassTransit();
        }

        public static void UseHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/HealthCheck", new HealthCheckOptions
            {
                Predicate = _ => _.Name != KaveNegarCheckName && _.Name != NikSmsCheckName,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                ResultStatusCodes = new Dictionary<HealthStatus, int>
                {
                    {HealthStatus.Healthy, 200},
                    {HealthStatus.Degraded, 200},
                    {HealthStatus.Unhealthy, 500}
                }
            });

            app.UseHealthChecks("/HealthCheck/KaveNegar", new HealthCheckOptions
            {
                Predicate = _ => _.Name == KaveNegarCheckName,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                ResultStatusCodes = new Dictionary<HealthStatus, int>
                {
                    {HealthStatus.Healthy, 200},
                    {HealthStatus.Degraded, 200},
                    {HealthStatus.Unhealthy, 500}
                }
            });
        }
    }
}