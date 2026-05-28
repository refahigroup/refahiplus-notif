using MassTransit;
using Refahi.Notif.Domain.Core.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var config = builder.Configuration;

        var brokerUsername = config["BrokerInfo:Username"];
        var brokerPassword = config["BrokerInfo:Password"];

        brokerUsername = !string.IsNullOrEmpty(brokerUsername)
           ? brokerUsername?.ReplaceWithEnvironmentVariables()
           : string.Empty;

        brokerPassword = !string.IsNullOrEmpty(brokerPassword)
           ? brokerPassword?.ReplaceWithEnvironmentVariables()
           : string.Empty;

        cfg.Host(config["BrokerInfo:Host"], "/", h =>
        {
            h.Username(brokerUsername!);
            h.Password(brokerPassword!);
        });

        cfg.ConfigureEndpoints(context);
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();

app.Run();
