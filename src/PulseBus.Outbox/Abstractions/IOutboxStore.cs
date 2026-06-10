namespace PulseBus.Outbox.Abstractions;

public interface IOutboxStore
{
    Task AddAsync(IOutboxMessage message, CancellationToken ct);
    Task<IReadOnlyList<IOutboxMessage>> GetPendingAsync(int max, CancellationToken ct);
    Task MarkAsProcessedAsync(Guid id, CancellationToken ct);
}