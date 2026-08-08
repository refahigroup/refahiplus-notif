using System.Text;
using DotNet.Testcontainers.Containers;
using RabbitMQ.Client;
using Testcontainers.RabbitMq;

namespace Refahi.Notif.Infrastructure.Consumer.Tests;

public sealed class RabbitMqVirtualHostIsolationTests
{
    private const string Username = "integration-user";
    private const string Password = "integration-password";
    private const string RefahiVirtualHost = "refahi-prod";
    private const string TochalVirtualHost = "tochal-prod";
    private const string SharedQueueName = "send-verify-sms";

    [Fact]
    [Trait("Category", "Integration")]
    public async Task IdenticallyNamedQueuesInSeparateVirtualHostsDoNotShareMessages()
    {
        await using var rabbitMq = new RabbitMqBuilder("rabbitmq:4.1-management-alpine").Build();
        await rabbitMq.StartAsync();

        await AssertCommandSucceeded(rabbitMq, "add_user", Username, Password);
        await AssertCommandSucceeded(rabbitMq, "add_vhost", RefahiVirtualHost);
        await AssertCommandSucceeded(rabbitMq, "add_vhost", TochalVirtualHost);
        await AssertCommandSucceeded(rabbitMq, "set_permissions", "-p", RefahiVirtualHost, Username, ".*", ".*", ".*");
        await AssertCommandSucceeded(rabbitMq, "set_permissions", "-p", TochalVirtualHost, Username, ".*", ".*", ".*");

        var brokerUri = new Uri(rabbitMq.GetConnectionString());
        await using var refahiConnection = await CreateConnection(brokerUri, RefahiVirtualHost);
        await using var tochalConnection = await CreateConnection(brokerUri, TochalVirtualHost);
        await using var refahiChannel = await refahiConnection.CreateChannelAsync();
        await using var tochalChannel = await tochalConnection.CreateChannelAsync();

        await refahiChannel.QueueDeclareAsync(SharedQueueName, durable: false, exclusive: false, autoDelete: false);
        await tochalChannel.QueueDeclareAsync(SharedQueueName, durable: false, exclusive: false, autoDelete: false);

        const string refahiPayload = "refahi-pattern-810889";
        const string tochalPayload = "tochal-pattern-812480";

        await refahiChannel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: SharedQueueName,
            mandatory: true,
            body: Encoding.UTF8.GetBytes(refahiPayload));
        await tochalChannel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: SharedQueueName,
            mandatory: true,
            body: Encoding.UTF8.GetBytes(tochalPayload));

        var refahiMessage = await refahiChannel.BasicGetAsync(SharedQueueName, autoAck: true);
        var tochalMessage = await tochalChannel.BasicGetAsync(SharedQueueName, autoAck: true);

        Assert.NotNull(refahiMessage);
        Assert.NotNull(tochalMessage);
        Assert.Equal(refahiPayload, Encoding.UTF8.GetString(refahiMessage.Body.Span));
        Assert.Equal(tochalPayload, Encoding.UTF8.GetString(tochalMessage.Body.Span));
        Assert.Null(await refahiChannel.BasicGetAsync(SharedQueueName, autoAck: true));
        Assert.Null(await tochalChannel.BasicGetAsync(SharedQueueName, autoAck: true));
    }

    private static Task<IConnection> CreateConnection(Uri brokerUri, string virtualHost)
    {
        var factory = new ConnectionFactory
        {
            HostName = brokerUri.Host,
            Port = brokerUri.Port,
            UserName = Username,
            Password = Password,
            VirtualHost = virtualHost
        };

        return factory.CreateConnectionAsync();
    }

    private static async Task AssertCommandSucceeded(
        RabbitMqContainer container,
        params string[] arguments)
    {
        var command = new[] { "rabbitmqctl" }.Concat(arguments).ToArray();
        var result = await container.ExecAsync(command);

        Assert.True(
            result.ExitCode == 0,
            $"Command '{string.Join(' ', command)}' failed. Stdout: {result.Stdout} Stderr: {result.Stderr}");
    }
}
