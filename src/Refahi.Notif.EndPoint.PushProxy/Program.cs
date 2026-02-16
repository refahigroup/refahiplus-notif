using Refahi.Notif.EndPoint.PushProxy.Startup;

var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;
services.ConfigureServices(builder.Configuration);



var app = builder.Build();
app.Configure(services.BuildServiceProvider());




