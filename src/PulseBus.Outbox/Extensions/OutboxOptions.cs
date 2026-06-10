namespace PulseBus.Outbox.Extensions;

public class OutboxOptions
{
    public int ProcessingIntervalMs { get; set; } = 500;
    public int MaxBatchSize { get; set; } = 50;
    public bool EnableMiddleware { get; set; } = true;
    public int MaxRetries { get; set; } = 3;
    public int RetryBackoffMs { get; set; } = 1000;
}