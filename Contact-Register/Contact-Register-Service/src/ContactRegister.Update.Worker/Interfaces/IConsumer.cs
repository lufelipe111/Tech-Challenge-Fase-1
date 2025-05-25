namespace ContactRegister.Update.Worker.Interfaces;

public interface IConsumer : IDisposable
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}