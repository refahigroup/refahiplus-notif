using MassTransit;
using Refahi.Notif.Application.Contract;
using Refahi.Notif.Application.Service;
using Refahi.Notif.Infrastructure.Messaging.Sms;
using Refahi.Notif.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddPersistance(builder.Configuration);
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host(builder.Configuration["BrokerInfo:Host"], "/", h =>
        {
            h.Username(builder.Configuration["BrokerInfo:Username"]);
            h.Password(builder.Configuration["BrokerInfo:Password"]);
        });

        cfg.ConfigureEndpoints(context);

    });
});

//builder.Services.AddConsumer();
builder.Services.AddSmsMessaging();
//builder.Services.AddEmailMessaging();
//builder.Services.AddPushNotificationMessaging();
builder.Services.AddApplicationContract();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
