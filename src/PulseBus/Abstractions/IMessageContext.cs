using System.Threading.Tasks;
using PulseBus.Models;

namespace PulseBus.Abstractions;

public interface IMessageContext
{
    string MessageId { get; }
    string CorrelationId { get; }
    MessageHeaders Headers { get; }
    Task AcknowledgeAsync();
    Task RejectAsync(bool requeue = false);
}