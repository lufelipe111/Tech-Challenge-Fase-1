using System.Net;
using System.Net.Http.Json;
using System.Text;
using ContactRegister.Application.DTOs;
using ContactRegister.Application.Inputs;
using ContactRegister.Infrastructure.Persistence;
using ContactRegister.Tests.IntegrationTests.Common;
using ContactRegister.Tests.IntegrationTests.TestContainers.Factories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.TestContainers.Tests;

public class RabbitTest : BaseIntegrationTests, IClassFixture<TestContainerContactRegisterFactory>
{
    public RabbitTest(TestContainerContactRegisterFactory factory) : base(factory)
    {
        var context = factory.Services.GetRequiredService<AppDbContext>();
        if (context.Database.GetPendingMigrations().Any())
            context.Database.Migrate();
    }

    [Fact(Timeout = 600_000)] // 10 seconds timeout for the test itself
    public async Task Should_Publish_And_Consume_Message()
    {
	    var factory = new ConnectionFactory
	    {
		    HostName = "localhost",
		    Port = 5672,
		    UserName = "guest",
		    Password = "guest"
	    };

	    const string queueName = "test-queue";
	    const string message = "Hello RabbitMQ!";
	    string? receivedMessage = null;

	    using var connection = await factory.CreateConnectionAsync();
	    using var channel = await connection.CreateChannelAsync();

	    await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false);

	    var body = Encoding.UTF8.GetBytes(message);
	    await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);

	    var consumer = new AsyncEventingBasicConsumer(channel);
	    var tcs = new TaskCompletionSource<string>();

	    consumer.ReceivedAsync += async (_, ea) =>
	    {
		    receivedMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
		    tcs.TrySetResult(receivedMessage);
	    };

	    await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

	    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
	    using (cts.Token.Register(() => tcs.TrySetCanceled()))
	    {
		    try
		    {
			    var result = await tcs.Task;
			    Assert.Equal(message, result);
		    }
		    catch (TaskCanceledException)
		    {
			    Assert.False(true, "Timed out waiting for message.");
		    }
	    }
    }
}