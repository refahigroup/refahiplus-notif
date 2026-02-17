using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Infrastructure.Messaging.RealTime.MassTransit;

namespace Refahi.Notif.Infrastructure.Messaging.RealTime
{
    public static class ConfigureService
    {
        public static void AddRealTimeMessaging(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            services.AddScoped<IRealTimeSender, RealTimeSender>();
            services.AddSingleton<RealTimeHub>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SendRealTimeMessageToAddressConsumer>();
                x.AddConsumer<SendRealTimeMessageToUserConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var config = serviceProvider.GetRequiredService<IConfiguration>();

                    cfg.Host(config["BrokerInfo:Host"], "/", h =>
                    {
                        h.Username(config["BrokerInfo:Username"]);
                        h.Password(config["BrokerInfo:Password"]);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddHostedService<MassTransitConsoleHostedService>();
        }
    }
}