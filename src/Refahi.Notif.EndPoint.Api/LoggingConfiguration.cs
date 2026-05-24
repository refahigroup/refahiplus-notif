using Refahi.Notif.Domain.Core.Utility;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Refahi.Notif.EndPoint.Api
{
    //public class WithRequest : ILogEventEnricher
    //{
    //    IServiceProvider _provider;
    //    public WithRequest(IServiceProvider provider)
    //    {
    //        _provider = provider;
    //    }
    //    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    //    {
    //        var context = _provider.GetService<IHttpContextAccessor>().HttpContext;
    //        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Request",context.Request.Path));
    //    }
    //}
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
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        ModifyConnectionSettings = (c) => c.BasicAuthentication(elasticUser, elasticPassword),
                        IndexFormat = elasticIndexFormat,
                        MinimumLogEventLevel = LogEventLevel.Debug
                    });
            }
            #endregion
        };

    }
}
