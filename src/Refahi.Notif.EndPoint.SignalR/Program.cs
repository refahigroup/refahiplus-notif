using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Refahi.Notif.EndPoint.SignalR;
using Refahi.Notif.Infrastructure.Messaging.RealTime;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRealTimeMessaging();
builder.Services.AddCustomAuthentication(builder.Configuration);


builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();


builder.Host.UseSerilog(LoggingConfiguration.ConfigureLogger);

var app = builder.Build();

app.UseCors(x =>
{
    x.AllowCredentials();
    x.AllowAnyMethod();
    x.AllowAnyHeader();

    var allowOrigins = app.Configuration.GetSection("AllowOrigins").Get<string[]>();
    x.WithOrigins(allowOrigins);
});
app.Use((x, y) =>
{
    //x.Response.Headers.Add("Access-Control-Allow-Origin", app.Configuration["AllowOrigins"]);
    //x.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
    //x.Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,OPTIONS,PUT,DELETE,PATCH");
    //x.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-Requested-With, Authorization, x-signalr-user-agent");

    if (x.Request.Method == "OPTIONS")
        x.Response.WriteAsync("a");

    return y.Invoke(x);
});





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}
else
    app.UseDeveloperExceptionPage();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", x =>
    {
        var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
        logger.LogInformation("Test Information In SignalR Local");

        if (!x.User.Identity.IsAuthenticated)
            return x.Response.WriteAsync("signalR Home page");

        return x.Response.WriteAsync($"signalR Home page :{x.User.FindFirst("sub")?.Value}");
    });

    endpoints.MapHub<RealTimeHub>("/RealTimeHub", options =>
    {
        options.Transports = HttpTransportType.WebSockets;
    });
});


app.Run();

public class NameUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User.FindFirst("sub")?.Value;
    }
}
