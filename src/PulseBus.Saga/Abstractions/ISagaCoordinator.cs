namespace PulseBus.Saga.Abstractions;

public interface ISagaCoordinator
{
    Task HandleEventAsync(object evt, SagaEventMetadata metadata, CancellationToken ct);
}