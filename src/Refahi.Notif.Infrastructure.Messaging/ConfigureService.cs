using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Infrastructure.Messaging.PushNotification.APN;
using Refahi.Notif.Infrastructure.Messaging.PushNotification.Firebase;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Infrastructure.Messaging.PushNotification
{
    public static class ConfigureService
    {
        public static void AddPushNotificationMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<FirebaseNotificationSender>();

            services.AddScoped<APNNotificationSender>();
            services.AddScoped<APNProxyNotificationSender>();



            #region Config

            services.AddConfiguration<FirebaseConfiguration>(configuration, "FirebaseConfiguration");
            services.AddConfiguration<APNConfiguration>(configuration, "APNConfiguration");


            #endregion

            services.AddScoped<PushNotificationServiceResolver>(serviceProvider => (deviceType) =>
            {
                switch (deviceType)
                {
                    default:
                    case DeviceType.Firebase:
                        {
                            return serviceProvider.GetService<FirebaseNotificationSender>();
                        }
                    case DeviceType.APN:
                        {
                            var configuration = serviceProvider.GetService<APNConfiguration>();
                            return configuration.UseProxy ?
                                    serviceProvider.GetService<APNProxyNotificationSender>() :
                                    serviceProvider.GetService<APNNotificationSender>();
                        }
                        break;
                }
            });
        }

        private static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string name) where T : class
        {
            if (configuration.GetChildren().Any(x => x.Key == name))
                services.AddSingleton(configuration.GetSection(name).Get<T>());
        }

        public static void UsePushNotificationMessaging(this IApplicationBuilder app)
        {

        }
    }
}
