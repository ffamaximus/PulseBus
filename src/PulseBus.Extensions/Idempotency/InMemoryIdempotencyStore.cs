using System.Collections.Generic;
using System.Threading.Tasks;
using PulseBus.Abstractions;

namespace PulseBus.Extensions.Idempotency;

public class InMemoryIdempotencyStore : IIdempotencyStore
{
    private readonly HashSet<string> _processed = new();

    public Task<bool> ExistsAsync(string messageId)
        => Task.FromResult(_processed.Contains(messageId));

    public Task MarkAsync(string messageId)
    {
        _processed.Add(messageId);
        return Task.CompletedTask;
    }
}