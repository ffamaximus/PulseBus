using PulseBus.Saga.Abstractions;

namespace PulseBus.Saga.Core;

public class SagaStepBuilder<TState>(SagaStep step)
    where TState : ISagaState
{
    public SagaStepBuilder<TState> Then<TCommand>()
    {
        step.CommandType = typeof(TCommand);
        return this;
    }

    public SagaStepBuilder<TState> On<TEvent>()
    {
        step.NextEventType = typeof(TEvent);
        return this;
    }

    public void Complete()
    {
        step.IsFinal = true;
    }
}
