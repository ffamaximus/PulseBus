using PulseBus.Saga.Abstractions;

namespace PulseBus.Saga.Persistence;

public class SagaStateEntity : ISagaState
{
    public Guid Id { get; set; }
    public string Status { get; set; } = "Running";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string Data { get; set; } = "{}";
}
