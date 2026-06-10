using PulseBus.Saga.Abstractions;

namespace PulseBus.Saga.Core;

public class SagaBuilder<TState> : ISagaBuilder
    where TState : ISagaState
{
    private readonly List<SagaStep> _steps = new();

    public SagaStepBuilder<TState> StartWith<TEvent>()
    {
        var step = new SagaStep(typeof(TEvent));
        _steps.Add(step);
        return new SagaStepBuilder<TState>(step);
    }

    public void AddStep(SagaStep step)
    {
        _steps.Add(step);
    }

    public IReadOnlyList<SagaStep> Build() => _steps;
}

