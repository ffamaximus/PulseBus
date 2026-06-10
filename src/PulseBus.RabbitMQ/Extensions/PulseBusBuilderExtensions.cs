using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PulseBus.Abstractions;
using PulseBus.Attributes;
using PulseBus.Builders;
using PulseBus.Extensions.Middlewares;
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

        // var connection = new RabbitMqConnection(options);
        // 🔥 Registrar la conexión en DI
        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<RabbitMqConnection>();

        builder.Options.Provider =
            new RabbitMqBusProvider(builder.Services.BuildServiceProvider().GetRequiredService<RabbitMqConnection>(),
                builder.Options);
        
        builder.Services.AddSingleton<IBusProvider, RabbitMqBusProvider>();
        return builder;
    }

    public static Task SubscribeListenerAsync<TListener, TMessage>(
        this IMessageBus bus,
        IServiceProvider services)
        where TListener : class
    {
        var listener = services.GetRequiredService<TListener>();
        var connection = services.GetRequiredService<RabbitMqConnection>();
        var options = services.GetRequiredService<BusOptions>();
        var type = typeof(TListener);

        // Leer atributos
        var queueAttr = type.GetCustomAttribute<QueueAttribute>();
        var deadLetterAttr = type.GetCustomAttribute<DeadLetterAttribute>();
        var retryAttr = type.GetCustomAttribute<RetryAttribute>();
        var prefetchAttr = type.GetCustomAttribute<PrefetchAttribute>();

        var queueName = queueAttr?.Name ?? typeof(TMessage).Name;
        
        // Middlewares locales por consumer
        var middlewares = options.Middlewares.ToList();

        if (retryAttr != null)
        {
            middlewares.Add(new AttributeRetryMiddleware(
                retryAttr.Attempts,
                retryAttr.DelaySeconds
            ));
        }

        var consumer = new RabbitMqConsumer(
            connection: connection,
            topic: queueName,
            handler: async (envelope, ctx) =>
            {
                var message = options.Serializer.Deserialize<TMessage>(envelope.Payload);
                await ((dynamic)listener).HandleAsync((dynamic)message, ctx);
            },
            options with { Middlewares = middlewares }, // middlewares locales,
            prefetchAttr: prefetchAttr,
            deadLetterAttr: deadLetterAttr
        );

        return consumer.StartAsync();
    }
}