namespace ContactRegister.Infrastructure.Messaging.Configuration;

public class RabbitMqConfiguration
{
    public string HostName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // Exchange & queue settings
    public string ExchangeName { get; set; } = string.Empty;
    public string ExchangeType { get; set; } = string.Empty; 
    public string RoutingKey { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;

    // Durable or ephemeral
    public bool Durable { get; set; } = true;
    public bool AutoDelete { get; set; } = false;
}