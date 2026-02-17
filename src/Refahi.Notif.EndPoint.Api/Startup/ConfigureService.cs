using FluentValidation.AspNetCore;
using Hangfire;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Prometheus;
using Swashbuckle.AspNetCore.SwaggerGen;
using Refahi.Notif.Infrastructure.RedisCache;
using Refahi.Notif.Infrastructure.MemoryCache;
using Refahi.Notif.Application.Contract;
using Refahi.Notif.Application.Contract.Services;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Infrastructure.Messaging.Email;
using Refahi.Notif.Infrastructure.Persistence;
using Refahi.Notif.Infrastructure.Messaging.Sms;
using Refahi.Notif.Infrastructure.Persistence.Postgres;
using Refahi.Notif.Infrastructure.Messaging.Telegram;
using Refahi.Notif.Infrastructure.Consumer;
using Refahi.Notif.Application.Service;
using Refahi.Notif.Infrastructure.Messaging.PushNotification;
using Refahi.Notif.EndPoint.Api.Startup.Swagger;
using Refahi.Notif.EndPoint.Api.Startup.Authentication;

namespace Refahi.Notif.EndPoint.Api.Startup
{
    public static class ConfigureService
    {
        public static void ConfigureServices(this IServiceCollection services, ConfigurationManager configuration, IHostEnvironment env)
        {
            //public services
            services.AddControllers()
                .AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<SendMessageRequest>()
                );

            services.AddEndpointsApiExplorer();

            services.AddTransient<IIdentityService, IdentityService>();
            services.AddHttpContextAccessor();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = false; // Enable automatic validation
                
                // Custom response for model validation errors
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .SelectMany(e => e.Value!.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToArray();

                    var response = new
                    {
                        statusCode = 400,
                        message = "Validation failed",
                        errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });
            services.AddCors();

            services.AddHttpClient(Options.DefaultName).UseHttpClientMetrics(); ;




            services.AddCustomAuthentication(configuration).AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            })
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddCustomSwagger(configuration); ;


            //internal services
            // Use in-memory cache for Development, Redis for Production
            if (env.IsDevelopment())
            {
                services.AddInMemoryCache(configuration);
            }
            else
            {
                services.AddRedis(configuration);
            }

            services.AddConsumer(configuration);
            services.AddSmsMessaging();
            services.AddEmailMessaging(configuration);
            services.AddTelegramMessaging(configuration);
            services.AddPushNotificationMessaging(configuration);

            services.AddPersistance(configuration);
            //services.UseSqlServerAndHangfire(configuration);
            services.UsePostreSqlAndHangfire(configuration);

            services.AddApplicationContract();
            services.AddApplication(configuration);
            //services.AddCommandApplication();
            services.AddHealthCheck(configuration).ForwardToPrometheus();

            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    // .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
            //    .AddEnvironmentVariables();
        }

    }
}
