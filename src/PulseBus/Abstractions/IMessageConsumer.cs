using System.Threading;
using System.Threading.Tasks;

namespace PulseBus.Abstractions;

public interface IMessageConsumer
{
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}