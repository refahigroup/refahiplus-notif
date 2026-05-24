using Refahi.Notif.Domain.Core.Utility;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Refahi.Notif.EndPoint.SignalR
{
    public static class LoggingConfiguration
    {
        public static Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> ConfigureLogger =>
        (context, provider, configuration) =>
        {
            #region Enriching Logger Context
            var env = context.HostingEnvironment;

            configuration
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                .Enrich.WithProperty("Environment", env.EnvironmentName)
                .Enrich.WithExceptionDetails()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                //.Enrich.With(new WithRequest(provider))
                ;
            #endregion
            configuration.WriteTo.Console().MinimumLevel.Information();

            #region ElasticSearch Configuration.
            var elasticUrl = context.Configuration["ElasticLogging:ElasticUrl"];
            var elasticUser = context.Configuration["ElasticLogging:ElasticUser"];
            var elasticPassword = context.Configuration["ElasticLogging:ElasticPassword"];

            elasticUser = !string.IsNullOrEmpty(elasticUser)
                ? elasticUser.ReplaceWithEnvironmentVariables()
                : string.Empty;

            elasticPassword = !string.IsNullOrEmpty(elasticPassword)
                ? elasticPassword.ReplaceWithEnvironmentVariables()
                : string.Empty;


            if (!string.IsNullOrEmpty(elasticUrl))
            {
                var elasticIndexFormat = context.Configuration["ElasticLogging:IndexFormat"];

                configuration.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticUrl))
                    {
                        AutoRegisterTemplate = true,
                        ModifyConnectionSettings = (c) => c.BasicAuthentication(elasticUser, elasticPassword),
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        IndexFormat = elasticIndexFormat,
                        MinimumLogEventLevel = LogEventLevel.Debug
                    }); ;
            }
            #endregion
        };

    }
}
