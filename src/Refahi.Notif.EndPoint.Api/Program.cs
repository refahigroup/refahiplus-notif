using Refahi.Notif.EndPoint.Api;
using Refahi.Notif.EndPoint.Api.Startup;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;
services.ConfigureServices(builder.Configuration, builder.Environment);

builder.Host.UseSerilog(LoggingConfiguration.ConfigureLogger);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
});

var app = builder.Build();
app.Configure(services.BuildServiceProvider());




