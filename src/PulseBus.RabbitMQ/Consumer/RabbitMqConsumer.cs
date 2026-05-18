using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PulseBus.Abstractions;
using PulseBus.Models;
using PulseBus.Pipeline;
using PulseBus.RabbitMQ.Connection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PulseBus.RabbitMQ.Consumer;

public class RabbitMqConsumer(
    RabbitMqConnection connection,
    string topic,
    Func<MessageEnvelope, IMessageContext, Task> handler,
    BusOptions options)
    : IMessageConsumer
{
    private IChannel _channel;

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _channel = await connection.CreateChannelAsync();
        await _channel.QueueDeclareAsync(topic, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += OnMessageReceived;

        await _channel.BasicConsumeAsync(topic, autoAck: false, consumer, cancellationToken);
    }

    private async Task OnMessageReceived(object sender, BasicDeliverEventArgs args)
    {
        var envelope = new MessageEnvelope
        {
            MessageId = args.BasicProperties.MessageId,
            CorrelationId = args.BasicProperties.CorrelationId,
            Topic = topic,
            Payload = args.Body.ToArray(),
            Headers = (MessageHeaders)( args.BasicProperties.Headers?.ToDictionary(
                    h => h.Key,
                    h => h.Value.ToString()
                ) ?? new Dictionary<string, string>())
            
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
            var message = options.Serializer.Deserialize<MessageEnvelope>(ctx.Envelope.Payload);
            await handler(message, ctx.MessageContext);
        };

        foreach (var middleware in options.Middlewares.Reverse())
        {
            var current = next;
            next = (ctx) => middleware.InvokeAsync(ctx, current);
        }

        await next(middlewareContext);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        _channel.CloseAsync(cancellationToken);
        return Task.CompletedTask;
    }
}
