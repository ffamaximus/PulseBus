namespace PulseBus.Saga.Abstractions;

public class SagaEventMetadata(Guid correlationId, IDictionary<string, object>? headers = null)
{
    public Guid CorrelationId { get; } = correlationId;
    public IDictionary<string, object> Headers { get; } = headers ?? new Dictionary<string, object>();
}