namespace PulseBus.Saga.Core;

public class SagaStep(Type eventType)
{
    public Type EventType { get; } = eventType;
    public Type? CommandType { get; set; }
    public Type? NextEventType { get; set; }
    public bool IsFinal { get; set; }
}