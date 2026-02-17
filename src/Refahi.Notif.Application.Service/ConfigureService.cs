using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refahi.Notif.Application.Contract.Configuration;
using Refahi.Notif.Application.Contract.Dtos.Inbox;
using Refahi.Notif.Application.Service.Inbox.Common;
using Refahi.Notif.Application.Service.Message.Common;
using Refahi.Notif.Infrastructure.Messaging.Sms;
using Refahi.Notif.Infrastructure.Messaging.Sms.KaveNegar;
using Refahi.Notif.Infrastructure.Messaging.Sms.Mediana;
using Refahi.Notif.Infrastructure.Messaging.Sms.MedianaSMSHub;
using Refahi.Notif.Infrastructure.Messaging.Sms.Nik;
using Refahi.Notif.Infrastructure.Messaging.Sms.Shatel;
using StackExchange.Redis;

namespace Refahi.Notif.Application.Service
{
    public static class ConfigureService
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IInboxMessageService, InboxMessageService>();


            #region Config

            services.Configure<OtpConfiguration>(configuration.GetSection("OtpConfiguration"));

            services.AddConfiguration<SmsTemplate>(configuration, "SmsTemplate");

            services.AddConfiguration<NikSmsConfiguration>(configuration, "NikSmsConfiguration");

            services.AddConfiguration<KaveSmsConfiguration>(configuration, "KaveSmsConfiguration");

            services.AddConfiguration<MedianaSmsConfiguration>(configuration, MedianaSmsConfiguration.SectionName);

            services.AddConfiguration<ShatelSmsConfiguration>(configuration, "ShatelSmsConfiguration");

            services.Configure<MedianaHubSmsConfiguration>(configuration.GetSection("MedianaSmsHubConfiguration"));

            services.Configure<MessageSenderProviderConfig>(configuration.GetSection("MessageSenderProviderConfig"));

            var array = new List<string>();
            //TODO read from config
            var configurationOptions = new ConfigurationOptions();
            foreach (var endpoint in configuration.GetSection("RedisConfig:Endpoints").Get<string[]>())
            {
                configurationOptions.EndPoints.Add(endpoint);

            }
            configurationOptions.Password = configuration.GetSection("RedisConfig:Password").Get<string>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = configurationOptions;
                options.InstanceName = configuration.GetSection("RedisConfig:InstanceName").Get<string>();

            });

            services.AddHttpClient("MedianaSmsHub",
                    config =>
                    {
                        var baseUrl = configuration["MedianaSmsHubConfiguration:BaseUrl"];
                        config.BaseAddress = new Uri(baseUrl);
                        config.Timeout = TimeSpan.FromSeconds(30);
                    })
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = ((_, _, _, _) => true)!
                }
                ).SetHandlerLifetime(Timeout.InfiniteTimeSpan);

            #endregion


        }

        private static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string name) where T : class
        {
            if (configuration.GetChildren().Any(x => x.Key == name))
                services.AddSingleton(configuration.GetSection(name).Get<T>());
        }

        public static void UseApplication(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard();
        }

    }


}
