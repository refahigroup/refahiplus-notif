using Microsoft.Extensions.Configuration;
using Refahi.Notif.Infrastructure.Consumer;

namespace Refahi.Notif.Infrastructure.Consumer.Tests;

public sealed class RabbitMqBrokerSettingsTests
{
    [Fact]
    public void FromConfiguration_UsesRootVhostOutsideProductionWhenNotConfigured()
    {
        var configuration = BuildConfiguration();

        var settings = RabbitMqBrokerSettings.FromConfiguration(
            configuration,
            requireExplicitVirtualHost: false);

        Assert.Equal("/", settings.VirtualHost);
    }

    [Fact]
    public void FromConfiguration_UsesExplicitAndTrimmedVhost()
    {
        var configuration = BuildConfiguration(("BrokerInfo:VirtualHost", " tochal-prod "));

        var settings = RabbitMqBrokerSettings.FromConfiguration(
            configuration,
            requireExplicitVirtualHost: true);

        Assert.Equal("tochal-prod", settings.VirtualHost);
    }

    [Fact]
    public void FromConfiguration_RejectsMissingVhostInProduction()
    {
        var configuration = BuildConfiguration();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            RabbitMqBrokerSettings.FromConfiguration(
                configuration,
                requireExplicitVirtualHost: true));

        Assert.Contains("BrokerInfo:VirtualHost", exception.Message);
    }

    private static IConfiguration BuildConfiguration(params (string Key, string Value)[] overrides)
    {
        var values = new Dictionary<string, string?>
        {
            ["BrokerInfo:Host"] = "infra_rabbitmq",
            ["BrokerInfo:Username"] = "notif-user",
            ["BrokerInfo:Password"] = "not-a-real-password"
        };

        foreach (var (key, value) in overrides)
            values[key] = value;

        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }
}
