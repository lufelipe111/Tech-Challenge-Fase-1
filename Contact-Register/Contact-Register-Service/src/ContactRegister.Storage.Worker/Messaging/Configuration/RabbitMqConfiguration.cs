namespace ContactRegister.Storage.Worker.Messaging.Configuration;

public class RabbitMqConfiguration
{
    public string HostName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // Queue settings
    public string ExchangeName { get; set; } = string.Empty;
    public string RoutingKey { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
    public bool Durable { get; set; } = true;
    public bool Exclusive { get; set; } =  false;
    public bool AutoDelete { get; set; } = false;
}