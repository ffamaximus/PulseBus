namespace PulseBus.Outbox.Abstractions;

public interface IOutboxPublisher
{
    Task PublishAsync(object evt, CancellationToken ct);
}