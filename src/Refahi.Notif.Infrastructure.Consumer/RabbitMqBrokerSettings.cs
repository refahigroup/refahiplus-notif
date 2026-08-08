using Microsoft.Extensions.Configuration;

namespace Refahi.Notif.Infrastructure.Consumer;

public sealed record RabbitMqBrokerSettings(
    string Host,
    string VirtualHost,
    string Username,
    string Password)
{
    public const string SectionName = "BrokerInfo";

    public static RabbitMqBrokerSettings FromConfiguration(
        IConfiguration configuration,
        bool requireExplicitVirtualHost)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var section = configuration.GetSection(SectionName);
        var host = section["Host"];
        if (string.IsNullOrWhiteSpace(host))
            throw new InvalidOperationException("BrokerInfo:Host configuration is required.");

        var configuredVirtualHost = section["VirtualHost"];
        if (requireExplicitVirtualHost && string.IsNullOrWhiteSpace(configuredVirtualHost))
        {
            throw new InvalidOperationException(
                "BrokerInfo:VirtualHost must be explicitly configured in Production to prevent cross-environment queue sharing.");
        }

        var virtualHost = string.IsNullOrWhiteSpace(configuredVirtualHost)
            ? "/"
            : configuredVirtualHost.Trim();

        return new RabbitMqBrokerSettings(
            host.Trim(),
            virtualHost,
            section["Username"] ?? string.Empty,
            section["Password"] ?? string.Empty);
    }
}
