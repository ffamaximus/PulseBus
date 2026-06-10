using PulseBus.Saga.Core;

namespace PulseBus.Saga.Abstractions;

public interface ISagaBuilder
{
    void AddStep(SagaStep step);
}