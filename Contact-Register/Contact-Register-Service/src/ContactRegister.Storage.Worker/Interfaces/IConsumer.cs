namespace ContactRegister.Storage.Worker.Interfaces;

public interface IConsumer : IDisposable
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}