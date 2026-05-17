using System.Threading;
using System.Threading.Tasks;
using PulseBus.Models;

namespace PulseBus.Abstractions;

public interface IMessageProducer
{
    Task ProduceAsync(MessageEnvelope envelope, CancellationToken cancellationToken = default);
}