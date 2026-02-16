using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Refahi.Notif.Domain.Contract.Messaging;
using Refahi.Notif.Infrastructure.Messaging.Sms.KaveNegar;
using Refahi.Notif.Infrastructure.Messaging.Sms.Mediana;
using Refahi.Notif.Infrastructure.Messaging.Sms.MedianaSMSHub;
using Refahi.Notif.Infrastructure.Messaging.Sms.Nik;
using Refahi.Notif.Infrastructure.Messaging.Sms.Shatel;

namespace Refahi.Notif.Infrastructure.Messaging.Sms
{
    public static class ConfigureService
    {
        public static void AddSmsMessaging(this IServiceCollection services)
        {
            services.AddScoped<ISmsSender, KaveSmsSender>();
            services.AddScoped<ISmsSender, NikSmsSender>();
            services.AddScoped<ISmsSender, MedianaSmsSender>();
            services.AddScoped<ISmsSender, MedianaSmsHubSender>();
            services.AddScoped<ISmsSender, ShatelSmsSender>();
            services.AddScoped<ISmsSenderFactory, SmsSenderFactory>();

            services.AddSingleton<NikSmsUptimeChecker>();
            services.AddSingleton<KaveSmsCreditChecker>();

        }

        public static void UseSmsMessaging(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<NikSmsUptimeChecker>().Start();

        }
    }
}
