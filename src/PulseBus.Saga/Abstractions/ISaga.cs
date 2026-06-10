namespace PulseBus.Saga.Abstractions;

public interface ISaga
{
    ISagaState State { get; }
    Task HandleEventAsync(object evt, SagaEventMetadata metadata, CancellationToken ct);
}
