using Refahi.Notif.EndPoint.Api;
using Refahi.Notif.EndPoint.Api.Startup;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(LoggingConfiguration.ConfigureLogger);

builder.Services.ConfigureServices(builder.Configuration, builder.Environment);

WebApplication app = builder.Build();

app.Configure(app.Services);