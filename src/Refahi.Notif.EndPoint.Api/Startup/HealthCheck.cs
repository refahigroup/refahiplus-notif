using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
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
            var rabbitUrl = config["BrokerInfo:Host"] ?? "localhost";

            if (!rabbitUrl.Contains(':'))
                rabbitUrl += ":5672";

            return services.AddHealthChecks()
                .AddNpgSql((config["ConnectionStrings:PostgresNotif"] ?? "").ReplaceWithEnvironmentVariables())

                .AddCheck<KaveSmsCreditChecker>(KaveNegarCheckName)

                .AddHangfire(x =>
                {
                    x.MinimumAvailableServers = 1;
                })
                .AddRabbitMQ(sp =>
                {
                    var hostParts = rabbitUrl.Split(':');

                    var brokerUsername = config["BrokerInfo:Username"];
                    var brokerPassword = config["BrokerInfo:Password"];

                    brokerUsername = !string.IsNullOrEmpty(brokerUsername)
                        ? brokerUsername?.ReplaceWithEnvironmentVariables()
                        : string.Empty;

                    brokerPassword = !string.IsNullOrEmpty(brokerPassword)
                        ? brokerPassword?.ReplaceWithEnvironmentVariables()
                        : string.Empty;

                    var factory = new ConnectionFactory()
                    {
                        HostName = hostParts[0],
                        Port = hostParts.Length > 1 ? int.Parse(hostParts[1]) : 5672,
                        UserName = brokerUsername!,
                        Password = brokerPassword!
                    };

                    using (var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult())
                    {
                        return connection;
                    }
                });
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



//using HealthChecks.UI.Client;
//using Microsoft.AspNetCore.Diagnostics.HealthChecks;
//using Microsoft.Extensions.Diagnostics.HealthChecks;
//using RabbitMQ.Client;
//using Refahi.Notif.Infrastructure.Messaging.Sms.KaveNegar;
//using Refahi.Notif.Domain.Core.Utility;

//namespace Refahi.Notif.EndPoint.Api.Startup
//{
//    public static class HealthCheck
//    {
//        private static string KaveNegarCheckName = "KaveNegar";
//        private static string NikSmsCheckName = "NikSms";

//        public static IHealthChecksBuilder AddHealthCheck(this IServiceCollection services, IConfiguration config)
//        {
//            return services.AddHealthChecks()
//                .AddNpgSql((config["ConnectionStrings:PostgresNotif"] ?? "").ReplaceWithEnvironmentVariables())

//                .AddCheck<KaveSmsCreditChecker>(KaveNegarCheckName)

//                .AddHangfire(x =>
//                {
//                    x.MinimumAvailableServers = 1;
//                })

//                //.AddMassTransitBusHealthCheck();
//                .AddMassTransit();
//        }

//        public static void UseHealthCheck(this IApplicationBuilder app)
//        {
//            app.UseHealthChecks("/HealthCheck", new HealthCheckOptions
//            {
//                Predicate = _ => _.Name != KaveNegarCheckName && _.Name != NikSmsCheckName,
//                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
//                ResultStatusCodes = new Dictionary<HealthStatus, int>
//                {
//                    {HealthStatus.Healthy, 200},
//                    {HealthStatus.Degraded, 200},
//                    {HealthStatus.Unhealthy, 500}
//                }
//            });

//            app.UseHealthChecks("/HealthCheck/KaveNegar", new HealthCheckOptions
//            {
//                Predicate = _ => _.Name == KaveNegarCheckName,
//                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
//                ResultStatusCodes = new Dictionary<HealthStatus, int>
//                {
//                    {HealthStatus.Healthy, 200},
//                    {HealthStatus.Degraded, 200},
//                    {HealthStatus.Unhealthy, 500}
//                }
//            });
//        }
//    }
//}