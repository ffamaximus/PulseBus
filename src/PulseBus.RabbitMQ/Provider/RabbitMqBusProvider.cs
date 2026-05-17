using PulseBus.RabbitMQ.Connection;
using PulseBus.RabbitMQ.Consumer;
using PulseBus.RabbitMQ.Producer;

namespace PulseBus.RabbitMQ.Provider;

public class RabbitMqBusProvider : IBusProvider
{
    private readonly RabbitMqConnection _connection;
    private readonly BusOptions _options;

    public RabbitMqBusProvider(RabbitMqConnection connection, BusOptions options)
    {
        _connection = connection;
        _options = options;
    }

    public IMessageProducer CreateProducer()
        => new RabbitMqProducer(_connection, _options.Serializer);

    public IMessageConsumer CreateConsumer(string topic, Func<MessageEnvelope, IMessageContext, Task> handler)
        => new RabbitMqConsumer(_connection, topic, handler, _options);
}
