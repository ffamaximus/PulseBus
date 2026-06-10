using PulseBus.Outbox.Abstractions;

namespace PulseBus.Outbox.EFCore;

public class OutboxMessageEntity : IOutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
}