using Prometheus;
using Refahi.Notif.Application.Service;
using Refahi.Notif.Domain.Contract;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.EndPoint.Api.Middlewares;
using Refahi.Notif.EndPoint.Api.Startup.Swagger;
using Refahi.Notif.Infrastructure.Messaging.Email;
using Refahi.Notif.Infrastructure.Messaging.PushNotification;
using Refahi.Notif.Infrastructure.Messaging.Sms;
using Refahi.Notif.Infrastructure.Messaging.Telegram;
using Refahi.Notif.Infrastructure.Persistence.Postgres;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.EndPoint.Api.Startup
{
    public static class ConfigureApp
    {
        public static void Configure(this WebApplication app, IServiceProvider provider)
        {

            app.UseCors(x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                var allowOrigins = app.Configuration.GetSection("AllowOrigins").Get<string[]>();
                x.WithOrigins(allowOrigins);
            });
            app.UseCustomSwagger(provider);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ErrorHandlingMiddleware>();

            }
            app.UseRouting();
            app.UseHttpMetrics();

            app.UseApplication();
            app.UseSmsMessaging();
            app.UseEmailMessaging();
            //app.UseTelegramMessaging();
            //app.UsePushNotificationMessaging();
            //app.UseHttpsRedirection();

            //app.UseSqlServerHangfire(provider);
            //app.MigrateSqlServerDb(provider);

            app.MigratePostgreDb(provider);
            app.UsePostgreHangfire(provider);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.UseHealthCheck();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
            });

            //SeedSetting(app);

            string url = string.Empty;

            try
            {
                url = app.Configuration["Url"];
            }
            catch { }



            if (!string.IsNullOrEmpty(url))
                app.Run(url);
            else
                app.Run();
        }
        private static void SeedSetting(WebApplication app)
        {
            var serviceScopeFactory = app.Services.GetService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var settingsHolder = scope.ServiceProvider.GetRequiredService<ISettingsHolder>();

                foreach (var item in (SettingKey[])Enum.GetValues(typeof(SettingKey)))
                {
                    switch (item)
                    {
                        case SettingKey.DefaultSmsGateway:
                            settingsHolder.Set(item, $"enum:{nameof(SmsGateway)}", ((int)SmsGateway.Kavenegar).ToString());
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }
    }
}
