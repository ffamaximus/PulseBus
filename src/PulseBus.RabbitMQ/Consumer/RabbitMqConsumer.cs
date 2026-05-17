using PulseBus.RabbitMQ.Connection;
using RabbitMQ.Client.Events;

namespace PulseBus.RabbitMQ.Consumer;

public class RabbitMqConsumer : IMessageConsumer
{
    private readonly RabbitMqConnection _connection;
    private readonly string _topic;
    private readonly Func<MessageEnvelope, IMessageContext, Task> _handler;
    private readonly BusOptions _options;
    private IModel _channel;

    public RabbitMqConsumer(
        RabbitMqConnection connection,
        string topic,
        Func<MessageEnvelope, IMessageContext, Task> handler,
        BusOptions options)
    {
        _connection = connection;
        _topic = topic;
        _handler = handler;
        _options = options;
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        _channel = _connection.CreateChannel();
        _channel.QueueDeclare(_topic, durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += OnMessageReceived;

        _channel.BasicConsume(_topic, autoAck: false, consumer);

        return Task.CompletedTask;
    }

    private async Task OnMessageReceived(object sender, BasicDeliverEventArgs args)
    {
        var envelope = new MessageEnvelope
        {
            MessageId = args.BasicProperties.MessageId,
            CorrelationId = args.BasicProperties.CorrelationId,
            Topic = _topic,
            Payload = args.Body.ToArray(),
            Headers = new MessageHeaders(
                args.BasicProperties.Headers?.ToDictionary(
                    h => h.Key,
                    h => h.Value.ToString()
                ) ?? new Dictionary<string, string>()
            )
        };

        var context = new RabbitMqMessageContext(_channel, args);

        // Ejecutar pipeline
        var middlewareContext = new MiddlewareContext
        {
            Envelope = envelope,
            MessageContext = context
        };

        MiddlewareDelegate next = async (ctx) =>
        {
            var message = _options.Serializer.Deserialize<object>(ctx.Envelope.Payload);
            await _handler(message, ctx.MessageContext);
        };

        foreach (var middleware in _options.Middlewares.Reverse())
        {
            var current = next;
            next = (ctx) => middleware.InvokeAsync(ctx, current);
        }

        await next(middlewareContext);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        _channel?.Close();
        return Task.CompletedTask;
    }
}
