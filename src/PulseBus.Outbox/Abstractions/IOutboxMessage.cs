namespace PulseBus.Outbox.Abstractions;

public interface IOutboxMessage
{
    Guid Id { get; }
    string Type { get; }
    string Payload { get; }
    DateTime CreatedAt { get; }
    DateTime? ProcessedAt { get; }
}