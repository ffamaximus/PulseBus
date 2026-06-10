using System;
using System.Threading;
using System.Threading.Tasks;

namespace PulseBus.Abstractions;

public interface IMessageBus
{
    Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default);
    Task SubscribeAsync<T>(string topic, Func<T, IMessageContext, Task> handler);
    Task SendAsync(object command, CancellationToken ct = default);
}