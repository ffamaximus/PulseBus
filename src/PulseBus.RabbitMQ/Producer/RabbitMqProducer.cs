using PulseBus.RabbitMQ.Connection;

namespace PulseBus.RabbitMQ.Producer;

public class RabbitMqProducer : IMessageProducer
{
    private readonly RabbitMqConnection _connection;
    private readonly IMessageSerializer _serializer;

    public RabbitMqProducer(RabbitMqConnection connection, IMessageSerializer serializer)
    {
        _connection = connection;
        _serializer = serializer;
    }

    public Task ProduceAsync(MessageEnvelope envelope, CancellationToken cancellationToken = default)
    {
        using var channel = _connection.CreateChannel();

        var props = channel.CreateBasicProperties();
        props.MessageId = envelope.MessageId;
        props.CorrelationId = envelope.CorrelationId;
        props.Headers = envelope.Headers.ToDictionary(h => h.Key, h => (object)h.Value);

        channel.BasicPublish(
            exchange: "",
            routingKey: envelope.Topic,
            basicProperties: props,
            body: envelope.Payload
        );

        return Task.CompletedTask;
    }
}