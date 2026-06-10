namespace PulseBus.Saga.Abstractions;

public interface ISagaRepository
{
    Task<ISagaState?> GetAsync(Guid id, Type stateType, CancellationToken ct);
    Task SaveAsync(ISagaState state, CancellationToken ct);
}