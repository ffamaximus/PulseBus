namespace PulseBus.Outbox.Abstractions;

public interface IOutboxProcessor
{
    Task ProcessAsync(CancellationToken ct);
}