namespace PulseBus.Core.Abstractions;

public interface IIdempotencyStore
{
    Task<bool> ExistsAsync(string messageId);
    Task MarkAsync(string messageId);
}