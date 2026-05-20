using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PulseBus.Abstractions;
using PulseBus.Models;
using PulseBus.RabbitMQ.Connection;
using RabbitMQ.Client;

namespace PulseBus.RabbitMQ.Producer;

public class RabbitMqProducer(RabbitMqConnection connection, IMessageSerializer serializer)
    : IMessageProducer
{
    private readonly IMessageSerializer _serializer = serializer;

    public async Task ProduceAsync(MessageEnvelope envelope, CancellationToken cancellationToken = default)
    {
        var conn = await connection.GetConnectionAsync();
        await using var channel = await conn.CreateChannelAsync(cancellationToken: cancellationToken);

        var props = new BasicProperties
        {
            MessageId = envelope.MessageId,
            CorrelationId = envelope.CorrelationId,
            Headers = envelope.Headers.ToDictionary(h => h.Key, h => (object)h.Value)!,
            DeliveryMode = DeliveryModes.Persistent
        };

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: envelope.Topic,
            mandatory: true,
            basicProperties: props,
            body: envelope.Payload,
            cancellationToken: cancellationToken
        );
    }
}