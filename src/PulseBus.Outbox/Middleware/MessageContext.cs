using PulseBus.Pipeline;

namespace PulseBus.Outbox.Middleware;

public class MessageContext(
    object message,
    IDictionary<string, object>? metadata,
    CancellationToken cancellationToken)
    : MiddlewareContext
{
    public object Message { get; } = message;
    public IDictionary<string, object> Metadata { get; } = metadata ?? new Dictionary<string, object>();
    public CancellationToken CancellationToken { get; } = cancellationToken;
}

