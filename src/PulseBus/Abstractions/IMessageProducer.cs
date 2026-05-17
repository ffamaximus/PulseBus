using PulseBus.Core.Models;

namespace PulseBus.Core.Abstractions;

public interface IMessageProducer
{
    Task ProduceAsync(MessageEnvelope envelope, CancellationToken cancellationToken = default);
}