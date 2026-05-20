using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PulseBus.Abstractions;
using PulseBus.Attributes;
using PulseBus.Builders;
using PulseBus.Models;
using PulseBus.RabbitMQ.Connection;
using PulseBus.RabbitMQ.Consumer;
using PulseBus.RabbitMQ.Provider;

namespace PulseBus.RabbitMQ.Extensions;

public static class PulseBusRabbitMqExtensions
{
    public static PulseBusBuilder UseRabbitMq(
        this PulseBusBuilder builder,
        Action<RabbitMqConnectionOptions> configure)
    {
        var options = new RabbitMqConnectionOptions();
        configure(options);

        var connection = new RabbitMqConnection(options);

        builder.Options.Provider = new RabbitMqBusProvider(connection, builder.Options);

        return builder;
    }
    
    public static async Task SubscribeListenerAsync<TListener, TMessage>(
        IServiceProvider services)
        where TListener : class
    {
        var listener = services.GetRequiredService<TListener>();
        var bus = services.GetRequiredService<IMessageBus>();
        var connection = services.GetRequiredService<RabbitMqConnection>();
        var options = services.GetRequiredService<BusOptions>();
        var type = typeof(TListener);

        // Leer atributos
        var queueAttr = type.GetCustomAttribute<QueueAttribute>();
        var deadLetterAttr = type.GetCustomAttribute<DeadLetterAttribute>();
        var retryAttr = type.GetCustomAttribute<RetryAttribute>();
        var prefetchAttr = type.GetCustomAttribute<PrefetchAttribute>();

        var queueName = queueAttr?.Name ?? typeof(TMessage).Name;
        
        var consumer = new RabbitMqConsumer(
            connection: connection,
            topic: queueName,
            handler: async (envelope, ctx) =>
            {
                var message = options.Serializer.Deserialize<TMessage>(envelope.Payload);
                await ((dynamic)listener).HandleAsync((dynamic)message, ctx);
            },
            options: options,
            retryAttr: retryAttr,
            prefetchAttr: prefetchAttr,
            deadLetterAttr: deadLetterAttr
        );

        await consumer.StartAsync();
        // return bus.SubscribeAsync<TMessage>(
        //     queueName,
        //     async (msg, ctx) =>
        //     {
        //         await ((dynamic)listener).HandleAsync((dynamic)msg, ctx);
        //     });
    }
}
