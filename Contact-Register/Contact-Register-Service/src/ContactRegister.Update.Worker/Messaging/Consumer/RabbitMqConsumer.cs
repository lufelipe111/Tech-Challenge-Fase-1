﻿using System.Text;
using System.Text.Json;
using ContactRegister.Domain.Entities;
using ContactRegister.Shared.Interfaces.Repositories;
using ContactRegister.Shared.Messaging.Configuration;
using ContactRegister.Update.Worker.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ContactRegister.Update.Worker.Messaging.Consumer;

public class RabbitMqConsumer : IConsumer, IAsyncDisposable
{
    private readonly ILogger<RabbitMqConsumer> _logger;
    private readonly IConnection _connection;
    private readonly RabbitMqConfiguration _config;
    private readonly IServiceProvider _serviceProvider;

    private bool _disposed;
    private IChannel? _channel;
    private string? _consumerTag;

    public RabbitMqConsumer(
        ILogger<RabbitMqConsumer> logger,
        IConnection connection,
        IOptions<RabbitMqConfiguration> rabbitMqConfiguration,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _serviceProvider = serviceProvider;
        _config = rabbitMqConfiguration.Value ?? throw new ArgumentNullException(nameof(rabbitMqConfiguration));
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(RabbitMqConsumer));
        
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation("[x] Update message received: {Message}", message);
                var contact = JsonSerializer.Deserialize<Contact>(message);
                if (contact != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var contactRepository = scope.ServiceProvider.GetRequiredService<IContactRepository>();
                    await contactRepository.UpdateContactAsync(contact);
                }
                
                await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " Error: {Message}", ex.Message);
            }
        };
        
        _consumerTag = await _channel.BasicConsumeAsync(_config.QueueName, false, consumer, cancellationToken);
    }

    public void Dispose()
    {
        if (_channel?.IsOpen ?? false)
            _channel.CloseAsync().GetAwaiter().GetResult();
        _channel?.Dispose();
        _connection.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await (_channel?.BasicCancelAsync(_consumerTag ?? "") ?? Task.CompletedTask);
        await (_channel?.CloseAsync() ?? Task.CompletedTask);
        Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}