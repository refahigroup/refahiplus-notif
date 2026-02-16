using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refahi.Notif.Domain.Contract.Messaging;

namespace Refahi.Notif.Infrastructure.Messaging.Telegram
{
    public static class ConfigureService
    {
        public static void AddTelegramMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITelegramSender, TelegramSender>();


            #region Config

            services.AddConfiguration<TelegramConfiguration>(configuration, "TelegramConfiguration");


            #endregion
        }

        private static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string name) where T : class
        {
            if (configuration.GetChildren().Any(x => x.Key == name))
                services.AddSingleton(configuration.GetSection(name).Get<T>());
        }

        public static void UseTelegramMessaging(this IApplicationBuilder app)
        {

        }
    }
}
