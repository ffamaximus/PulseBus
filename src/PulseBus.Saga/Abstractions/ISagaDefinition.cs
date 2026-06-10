using PulseBus.Saga.Core;

namespace PulseBus.Saga.Abstractions;

public interface ISagaDefinition
{
    Type StateType { get; }
    IReadOnlyList<SagaStep> GetSteps();
}
