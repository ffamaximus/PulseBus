using System.Threading.Tasks;

namespace PulseBus.Abstractions;

public interface IIdempotencyStore
{
    Task<bool> ExistsAsync(string messageId);
    Task MarkAsync(string messageId);
}