using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refahi.Notif.Infrastructure.Consumer.InternalConsumers;
using Refahi.Notif.Infrastructure.Consumer.MassTransit;

namespace Refahi.Notif.Infrastructure.Consumer
{
    public static class ConfigureService
    {
        private static Action<IRetryConfigurator> LongRetry => x =>
            x.Exponential(20, TimeSpan.FromSeconds(1), TimeSpan.FromDays(7), TimeSpan.FromSeconds(10));

        private static Action<IRetryConfigurator> ShortRetry => x =>
            x.Exponential(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(2));


        public static void AddConsumer(this IServiceCollection services, IConfiguration config)
        {


            services.AddMassTransit(x =>
            {

                x.AddConsumerWithDefinition<DeleteMessageByTagConsumer>(x => x.UseMessageRetry(LongRetry));
                x.AddConsumerWithDefinition<DeleteMessageConsumer>(x => x.UseMessageRetry(LongRetry));
                x.AddConsumerWithDefinition<VerifySmsSentConsumer>(x => x.UseMessageRetry(ShortRetry));
                x.AddConsumerWithDefinition<SendMessageConsumer>(x => x.UseMessageRetry(LongRetry));
                x.AddConsumerWithDefinition<SendMessageToUserConsumer>(x => x.UseMessageRetry(LongRetry));
                x.AddConsumerWithDefinition<SendMessageToContactConsumer>(x => x.UseMessageRetry(LongRetry));
                x.AddConsumerWithDefinition<SendVerifySmsConsumer>(x =>
                {
                    x.UseMessageRetry(ShortRetry);
                    x.UseTimeout(x => x.Timeout = TimeSpan.FromSeconds(200));
                });


                x.AddConsumerWithDefinition<NotificationClickedConsumer>(x => x.UseMessageRetry(ShortRetry));
                x.AddConsumerWithDefinition<NotificationEventConsumer>(x => x.UseMessageRetry(ShortRetry));
                x.AddConsumerWithDefinition<NotificationDeliveredConsumer>(x => x.UseMessageRetry(ShortRetry));
                x.AddConsumerWithDefinition<SmsDeliveryConsumer>(x => x.UseMessageRetry(LongRetry));
                x.AddConsumerWithDefinition<UpsertDeviceConsumer>(x => x.UseMessageRetry(LongRetry));
                x.AddConsumerWithDefinition<FillUserInfoConsumer>(x => x.UseMessageRetry(LongRetry));
                x.AddConsumerWithDefinition<InvalidDeviceConsumer>(x => x.UseMessageRetry(LongRetry));
                x.AddConsumerWithDefinition<SetUserPhoneNumberConsumer>(x => x.UseMessageRetry(LongRetry));
                x.AddConsumerWithDefinition<SetUserEmailConsumer>(x => x.UseMessageRetry(LongRetry));


                //internal
                x.AddConsumerWithDefinition<SendSmsMessageConsumer>(x =>
                {
                    x.UseMessageRetry(LongRetry);
                    x.UseTimeout(x => x.Timeout = TimeSpan.FromSeconds(200));
                });
                x.AddConsumerWithDefinition<SendEmailMessageConsumer>(x => x.UseMessageRetry(LongRetry));
                x.AddConsumerWithDefinition<SendTelegramMessageConsumer>(x => x.UseMessageRetry(LongRetry));
                x.AddConsumerWithDefinition<SendPushMessageConsumer>(x => x.UseMessageRetry(ShortRetry));
                x.AddConsumerWithDefinition<SendNotificationMessageConsumer>(x => x.UseMessageRetry(ShortRetry));


                x.UsingRabbitMq((context, cfg) =>
                {

                    //cfg.UseMessageRetry(r =>
                    //   r.Exponential(
                    //       3,
                    //       TimeSpan.FromSeconds(1),
                    //       TimeSpan.FromSeconds(30),
                    //       TimeSpan.FromSeconds(5)));


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
        private static void AddConsumerWithDefinition<TConsumer>(this IBusRegistrationConfigurator x, Action<IConsumerConfigurator<TConsumer>> configure = null) where TConsumer : class, IConsumer
        {
            x.AddConsumer<TConsumer, CustomConsumerDefinition<TConsumer>>(configure);
        }
    }
    public class CustomConsumerDefinition<TConsumer> : ConsumerDefinition<TConsumer> where TConsumer : class, IConsumer
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<TConsumer> consumerConfigurator)
        {
            endpointConfigurator.DiscardFaultedMessages();
        }
    }
}
