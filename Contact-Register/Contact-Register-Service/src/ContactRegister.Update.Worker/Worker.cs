using ContactRegister.Update.Worker.Interfaces;

namespace ContactRegister.Update.Worker;

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
        _logger.LogInformation("Update worker running at: {Time}", DateTimeOffset.Now);
        await _consumer.ConsumeAsync(stoppingToken);
        _logger.LogInformation("Update worker stopped at: {Time}", DateTimeOffset.Now);
    }
}