namespace ContactRegister.Application.Interfaces.Messaging
{
    public interface IPublisher
    {
        Task PublishMessage(string message, string routingKey, CancellationToken? cancellationToken = default);
    }
}
