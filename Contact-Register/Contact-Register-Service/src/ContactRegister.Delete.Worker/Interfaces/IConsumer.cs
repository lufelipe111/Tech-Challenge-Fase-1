namespace ContactRegister.Delete.Worker.Interfaces;

public interface IConsumer : IDisposable
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}