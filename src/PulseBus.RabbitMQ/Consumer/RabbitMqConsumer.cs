#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PulseBus.Abstractions;
using PulseBus.Attributes;
using PulseBus.Extensions.Middlewares;
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
    BusOptions options,
    RetryAttribute? retryAttr = null,
    PrefetchAttribute? prefetchAttr = null,
    DeadLetterAttribute? deadLetterAttr = null)
    : IMessageConsumer
{
    private IChannel? _channel;

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var conn = await connection.GetConnectionAsync();
        _channel = await conn.CreateChannelAsync(cancellationToken: cancellationToken);
        
        var args = new Dictionary<string, object>();

        if (deadLetterAttr != null)
        {
            args["x-dead-letter-exchange"] = "";
            args["x-dead-letter-routing-key"] = deadLetterAttr.QueueName;
        }
        
        if (retryAttr != null)
        {
            options.Middlewares.Add(
                new AttributeRetryMiddleware(
                    retryAttr.Attempts,
                    retryAttr.DelaySeconds
                )
            );
        }


        await _channel.QueueDeclareAsync(
            queue: topic,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: args,
            cancellationToken: cancellationToken
        );
        
        if (prefetchAttr != null)
        {
            await _channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: prefetchAttr.Count,
                global: false,
                cancellationToken: cancellationToken
            );
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += OnMessageReceived;

        await _channel.BasicConsumeAsync(
            queue: topic,
            autoAck: false,
            consumer: consumer,
            cancellationToken: cancellationToken
        );
    }

    private async Task OnMessageReceived(object sender, BasicDeliverEventArgs args)
    {
        try
        {
            var envelope = new MessageEnvelope
            {
                MessageId = args.BasicProperties.MessageId,
                CorrelationId = args.BasicProperties.CorrelationId,
                Topic = topic,
                Payload = args.Body.ToArray(),
                Headers = (MessageHeaders)args.BasicProperties.Headers
            };

            var context = new RabbitMqMessageContext(_channel, args);

            var middlewareContext = new MiddlewareContext
            {
                Envelope = envelope,
                MessageContext = context
            };

            MiddlewareDelegate next = async (ctx) =>
            {
                await handler(ctx.Envelope, ctx.MessageContext);
            };

            foreach (var middleware in options.Middlewares.Reverse())
            {
                var current = next;
                next = (ctx) => middleware.InvokeAsync(ctx, current);
            }

            await next(middlewareContext);

            await _channel.BasicAckAsync(args.DeliveryTag, false);
        }
        catch (Exception)
        {
            await _channel.BasicNackAsync(args.DeliveryTag, false, true);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _channel.CloseAsync(cancellationToken);
    }
}