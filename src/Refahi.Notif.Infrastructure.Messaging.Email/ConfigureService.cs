using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refahi.Notif.Domain.Contract.Messaging;

namespace Refahi.Notif.Infrastructure.Messaging.Email
{
    public static class ConfigureService
    {
        public static void AddEmailMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailSender, EmailSender>();


            #region Config

            services.AddConfiguration<EmailConfiguration>(configuration, "EmailConfiguration");


            #endregion
        }

        private static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string name) where T : class
        {
            if (configuration.GetChildren().Any(x => x.Key == name))
                services.AddSingleton(configuration.GetSection(name).Get<T>());
        }

        public static void UseEmailMessaging(this IApplicationBuilder app)
        {

        }
    }
}
