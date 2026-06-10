namespace PulseBus.Saga.Abstractions;

public interface ISagaState
{
    Guid Id { get; set; }
    string Status { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
