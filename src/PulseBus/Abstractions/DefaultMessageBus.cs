using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using PulseBus.Models;

namespace PulseBus.Abstractions;

public class DefaultMessageBus(BusOptions options) : IMessageBus
{
    public Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
    {
        var producer = options.Provider.CreateProducer();

        var envelope = new MessageEnvelope
        {
            MessageId = Guid.NewGuid().ToString(),
            CorrelationId = Guid.NewGuid().ToString(),
            Topic = topic,
            Payload = options.Serializer.Serialize(message),
            Headers = new MessageHeaders()
        };

        return producer.ProduceAsync(envelope, cancellationToken);
    }

    public Task SubscribeAsync<T>(string topic, Func<T, IMessageContext, Task> handler)
    {
        var consumer = options.Provider.CreateConsumer(topic, async (env, ctx) =>
        {
            var message = options.Serializer.Deserialize<T>(env.Payload);
            await handler(message, ctx);
        });

        return consumer.StartAsync();
    }
    
    public Task SendAsync(object command, CancellationToken ct = default)
    {
        var type = command.GetType().Name;
        var envelope = new MessageEnvelope
        {
            MessageId = Guid.NewGuid().ToString(),
            CorrelationId = Guid.NewGuid().ToString(),
            Topic = type,
            Payload = JsonSerializer.SerializeToUtf8Bytes(command),
            Headers = new MessageHeaders()
        };

        var producer = options.Provider.CreateProducer();
        return producer.ProduceAsync(envelope, ct);
    }
}
