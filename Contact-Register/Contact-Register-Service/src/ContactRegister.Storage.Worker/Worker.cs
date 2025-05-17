using ContactRegister.Storage.Worker.Interfaces;

namespace ContactRegister.Storage.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConsumer _consumer;

    public Worker(ILogger<Worker> logger, IConsumer consumer)
    {
        _logger = logger;
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
        await _consumer.ConsumeAsync(stoppingToken);
        _logger.LogInformation("Worker stopped at: {Time}", DateTimeOffset.Now);
    }
}