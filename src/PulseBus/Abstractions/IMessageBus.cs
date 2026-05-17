namespace PulseBus.Core.Abstractions;

public interface IMessageBus
{
    Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default);
    Task SubscribeAsync<T>(string topic, Func<T, IMessageContext, Task> handler);
}