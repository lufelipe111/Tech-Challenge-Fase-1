
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using ContactRegister.Application.Interfaces.Messaging;
using ContactRegister.Infrastructure.Messaging.Configuration;

namespace ContactRegister.Infrastructure.Messaging.Publisher
{
    public sealed class RabbitMqPublisher : IPublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly RabbitMqConfiguration _config;
        private bool _disposed;

        public RabbitMqPublisher(IConnection connection, IOptions<RabbitMqConfiguration> config)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task PublishMessage(string message, string routingKey, CancellationToken? cancellationToken = default)
        {
            await using var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken ?? default);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                _config.ExchangeName, 
                routingKey, 
                false, 
                body, 
                cancellationToken ?? default);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // No finalizer, but this is good practice
        }
        
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _connection?.Dispose();
                }
                // No unmanaged resources, so nothing else to do here

                _disposed = true;
            }
        }
    }
}
