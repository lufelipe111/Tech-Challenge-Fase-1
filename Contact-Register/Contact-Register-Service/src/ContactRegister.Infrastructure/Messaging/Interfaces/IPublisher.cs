namespace ContactRegister.Infrastructure.Messaging.Publisher
{
    public interface IPublisher
    {
        Task PublishMessage(string message, CancellationToken cancellationToken);
    }
}
