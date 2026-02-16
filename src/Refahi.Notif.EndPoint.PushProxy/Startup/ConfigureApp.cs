using Refahi.Notif.Infrastructure.Messaging.PushNotification;

namespace Refahi.Notif.EndPoint.PushProxy.Startup
{
    public static class ConfigureApp
    {
        public static void Configure(this WebApplication app, IServiceProvider provider)
        {


            app.UseSwagger().UseSwaggerUI();


            app.UseDeveloperExceptionPage();


            app.UsePushNotificationMessaging();
            app.MapControllers();


            var url = app.Configuration["Url"];
            app.Run(url);
        }
    }
}
