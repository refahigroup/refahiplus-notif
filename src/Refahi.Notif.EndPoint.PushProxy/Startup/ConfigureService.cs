using Refahi.Notif.Infrastructure.Messaging.PushNotification;

namespace Refahi.Notif.EndPoint.PushProxy.Startup
{
    public static class ConfigureService
    {
        public static void ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddControllers();
            services.AddHttpClient();

            services.AddEndpointsApiExplorer();

            services.AddHttpContextAccessor();

            services.AddSwaggerGen();

            services.AddPushNotificationMessaging(configuration);
        }

    }
}
