using PulseBus.Saga.Abstractions;

namespace PulseBus.Saga.Core;

public abstract class SagaDefinition<TState> : ISagaDefinition
    where TState : ISagaState, new()
{
    private readonly SagaBuilder<TState> _builder = new();

    public Type StateType => typeof(TState);

    public abstract void Configure();

    public IReadOnlyList<SagaStep> GetSteps()
    {
        Configure();
        return _builder.Build();
    }

    protected SagaStepBuilder<TState> StartWith<TEvent>()
        => _builder.StartWith<TEvent>();
}