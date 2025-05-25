using ContactRegister.Shared.Messaging.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ContactRegister.Delete.Worker.Messaging.Service;

public class RabbitMqInitHostedService : IHostedService
{
    private readonly ILogger<RabbitMqInitHostedService> _logger;
    private readonly RabbitMqConfiguration _config;
    private IConnection _connection;

    public RabbitMqInitHostedService(
        ILogger<RabbitMqInitHostedService> logger, 
        IOptions<RabbitMqConfiguration> rabbitMqConfiguration)
    {
        _logger = logger;
        _config = rabbitMqConfiguration.Value;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("RabbitMQ Service is starting.");
        
        var connectionFactory = new ConnectionFactory
        {
            HostName = _config.HostName,
            UserName = _config.UserName,
            Password = _config.Password
        };
        
        _connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        
        await using var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
        if (!string.IsNullOrEmpty(_config.QueueName))
        {
            await channel.QueueDeclareAsync(
                _config.QueueName, 
                _config.Durable, 
                _config.Exclusive, 
                _config.AutoDelete,
                cancellationToken: cancellationToken);
            
            await channel.QueueBindAsync(
                _config.QueueName, 
                _config.ExchangeName, 
                _config.RoutingKey, 
                cancellationToken: cancellationToken);
        }
        
        await channel.CloseAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _connection.CloseAsync(cancellationToken);
        _connection.Dispose();
    }
}