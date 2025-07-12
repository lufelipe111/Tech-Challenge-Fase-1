using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using ContactRegister.Infrastructure.Messaging.Configuration;
using Microsoft.Extensions.Logging;

namespace ContactRegister.Infrastructure.Messaging.Service
{
    public class RabbitMqInitHostedService : IHostedService
    {
        private readonly ILogger<RabbitMqInitHostedService> _logger;
        private readonly IOptions<RabbitMqConfiguration> _config;
        private IConnection _connection;

        public RabbitMqInitHostedService(
            ILogger<RabbitMqInitHostedService> logger,
            IOptions<RabbitMqConfiguration> config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RabbitMQ Service is starting. Connecting to Host: {HostName}", _config.Value.HostName);
            var factory = new ConnectionFactory
            {
                HostName = _config.Value.HostName,
                UserName = _config.Value.UserName,
                Password = _config.Value.Password
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);

            await using var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Connected to RabbitMQ");

            if (!string.IsNullOrEmpty(_config.Value.ExchangeName))
            {
                await channel.ExchangeDeclareAsync(
                    exchange: _config.Value.ExchangeName,
                    type: _config.Value.ExchangeType,
                    durable: _config.Value.Durable,
                    autoDelete: _config.Value.AutoDelete,
                    arguments: null,
                    cancellationToken: cancellationToken
                );
                _logger.LogInformation("Exchange created: {ExchangeName} - {ExchangeType}", _config.Value.ExchangeName, _config.Value.ExchangeType);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _connection.CloseAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
            }
            _connection.Dispose();
        }
    }
}