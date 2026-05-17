using PulseBus.Core.Models;

namespace PulseBus.Core.Abstractions;

public interface IMessageContext
{
    string MessageId { get; }
    string CorrelationId { get; }
    MessageHeaders Headers { get; }
    Task AcknowledgeAsync();
    Task RejectAsync(bool requeue = false);
}