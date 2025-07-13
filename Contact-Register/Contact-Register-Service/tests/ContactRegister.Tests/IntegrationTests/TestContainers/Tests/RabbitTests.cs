using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContactRegister.Infrastructure.Messaging.Configuration;
using ContactRegister.Tests.IntegrationTests.Common;
using ContactRegister.Tests.IntegrationTests.TestContainers.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.TestContainers.Tests;

public class RabbitTest : BaseIntegrationTests, IClassFixture<TestContainerContactRegisterFactory>
{
	private readonly TestContainerContactRegisterFactory _factory;

	public RabbitTest(TestContainerContactRegisterFactory factory) : base(factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Should_Publish_And_Consume_Message()
	{
		var rabbitMqConfig = _factory.Services.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
		var factory = new ConnectionFactory
		{
			HostName = rabbitMqConfig.HostName,
			Port = rabbitMqConfig.Port,
			UserName = rabbitMqConfig.UserName,
			Password = rabbitMqConfig.Password
		};

		const string queueName = "test-queue";
		const string message = "Hello RabbitMQ!";

		using var connection = await factory.CreateConnectionAsync();
		using var channel = await connection.CreateChannelAsync();

		await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false);

		var consumer = new AsyncEventingBasicConsumer(channel);
		var autoResetEvent = new AutoResetEvent(false);
		string receivedMessage = null;

		consumer.ReceivedAsync += async (model, ea) =>
		{
			receivedMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
			autoResetEvent.Set();
		};

		await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

		var body = Encoding.UTF8.GetBytes(message);
		await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);

		var timedOut = !autoResetEvent.WaitOne(TimeSpan.FromSeconds(30));

		if (timedOut)
		{
			Assert.Fail("Timed out waiting for message.");
		}

		Assert.Equal(message, receivedMessage);
	}
}