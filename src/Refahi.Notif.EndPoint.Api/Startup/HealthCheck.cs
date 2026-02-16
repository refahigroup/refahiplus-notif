using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using Refahi.Notif.Infrastructure.Messaging.Sms.KaveNegar;

namespace Refahi.Notif.EndPoint.Api.Startup
{
    public static class HealthCheck
    {
        private static string KaveNegarCheckName = "KaveNegar";
        private static string NikSmsCheckName = "NikSms";
        public static IHealthChecksBuilder AddHealthCheck(this IServiceCollection services, ConfigurationManager config)
        {

            var rabbitUrl = config["BrokerInfo:Host"];

            if (!rabbitUrl.Contains(':'))
                rabbitUrl += ":5672";

            return services.AddHealthChecks()

             .AddSqlServer(config["ConnectionStrings:Notif"])

             .AddAsyncCheck(KaveNegarCheckName, x => services.BuildServiceProvider().GetService<KaveSmsCreditChecker>().Check())

             //.AddAsyncCheck(NikSmsCheckName, x => services.BuildServiceProvider().GetService<NikSmsUptimeChecker>().Check())

             .AddHangfire(x =>
             {
                 x.MinimumAvailableServers = 1;
                 //x.MaximumJobsFailed = 1;
             })
             .AddRabbitMQ(sp =>
             {
                 var hostParts = rabbitUrl.Split(':');
                 var factory = new ConnectionFactory()
                 {
                     HostName = hostParts[0],
                     Port = hostParts.Length > 1 ? int.Parse(hostParts[1]) : 5672,
                     UserName = config["BrokerInfo:Username"],
                     Password = config["BrokerInfo:Password"]
                 };
                 return factory.CreateConnectionAsync().GetAwaiter().GetResult();
             })
             ;

        }

        public static void UseHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/HealthCheck", new HealthCheckOptions
            {
                Predicate = _ => _.Name != KaveNegarCheckName && _.Name != NikSmsCheckName,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                ResultStatusCodes = new Dictionary<HealthStatus, int>
                {
                    {HealthStatus.Healthy,200},
                    {HealthStatus.Degraded,200},
                    {HealthStatus.Unhealthy,500}
                }
            });

            app.UseHealthChecks("/HealthCheck/KaveNegar", new HealthCheckOptions
            {
                Predicate = _ => _.Name == KaveNegarCheckName,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                ResultStatusCodes = new Dictionary<HealthStatus, int>
                {
                    {HealthStatus.Healthy,200},
                    {HealthStatus.Degraded,200},
                    {HealthStatus.Unhealthy,500}
                }
            });

            //app.UseHealthChecks("/HealthCheck/NikSms", new HealthCheckOptions
            //{
            //    Predicate = _ => _.Name == NikSmsCheckName,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            //    ResultStatusCodes = new Dictionary<HealthStatus, int>
            //    {
            //        {HealthStatus.Healthy,200},
            //        {HealthStatus.Degraded,200},
            //        {HealthStatus.Unhealthy,500}
            //    }
            //});
        }
    }
}
