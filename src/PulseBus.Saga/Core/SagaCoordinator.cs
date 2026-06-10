using PulseBus.Abstractions;
using PulseBus.Saga.Abstractions;

namespace PulseBus.Saga.Core;

public class SagaCoordinator(
    IEnumerable<ISagaDefinition> definitions,
    ISagaRepository repository,
    IMessageBus bus)
    : ISagaCoordinator
{
    public async Task HandleEventAsync(object evt, SagaEventMetadata metadata, CancellationToken ct)
    {
        foreach (var def in definitions)
        {
            var steps = def.GetSteps();

            // 1. Encontrar el paso que corresponde al evento recibido
            var step = steps.FirstOrDefault(s => s.EventType == evt.GetType());
            if (step == null)
                continue;

            // 2. Obtener o crear el estado de la Saga
            var state = await LoadOrCreateState(def, metadata.CorrelationId, ct);

            // 3. Ejecutar el comando del paso (si existe)
            if (step.CommandType != null)
            {
                var cmd = CreateCommand(step.CommandType, metadata.CorrelationId);
                await bus.SendAsync(cmd, ct);
            }

            // 4. Si es el paso final, marcar la Saga como completada
            if (step.IsFinal)
            {
                state.Status = "Completed";
            }

            // 5. Actualizar el estado
            state.UpdatedAt = DateTime.UtcNow;
            await repository.SaveAsync(state, ct);
        }
    }

    private async Task<ISagaState> LoadOrCreateState(
        ISagaDefinition def,
        Guid correlationId,
        CancellationToken ct)
    {
        var state = await repository.GetAsync(correlationId, def.StateType, ct);

        if (state != null)
            return state;

        // Crear nuevo estado
        var newState = (ISagaState)Activator.CreateInstance(def.StateType)!;
        newState.Id = correlationId;
        newState.Status = "Running";
        newState.CreatedAt = DateTime.UtcNow;
        newState.UpdatedAt = DateTime.UtcNow;

        await repository.SaveAsync(newState, ct);
        return newState;
    }

    private object CreateCommand(Type commandType, Guid correlationId)
    {
        // Asumimos que el comando tiene un constructor con (Guid sagaId)
        return Activator.CreateInstance(commandType, correlationId)!;
    }

}

