namespace PulseBus.Core.Abstractions;

public interface IMessageConsumer
{
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}