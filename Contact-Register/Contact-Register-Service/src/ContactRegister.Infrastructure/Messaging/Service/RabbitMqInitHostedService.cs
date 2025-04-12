using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using ContactRegister.Infrastructure.Messaging.Configuration;

namespace ContactRegister.Infrastructure.Messaging.Service
{
    public class RabbitMqInitHostedService : IHostedService
    {
        private readonly IOptions<RabbitMqConfiguration> _config;
        private IConnection _connection;

        public RabbitMqInitHostedService(IOptions<RabbitMqConfiguration> config)
        {
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.Value.HostName,
                UserName = _config.Value.UserName,
                Password = _config.Value.Password
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);

            await using var channel = await _connection.CreateChannelAsync(null, cancellationToken);
                
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
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connection.CloseAsync(cancellationToken);
            _connection.Dispose();
        }
    }
}