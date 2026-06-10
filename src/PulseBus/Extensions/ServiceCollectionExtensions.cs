using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using PulseBus.Abstractions;
using PulseBus.Builders;
using PulseBus.Models;

namespace PulseBus.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPulseBus(
        this IServiceCollection services,
        Action<PulseBusBuilder> configure)
    {
        var builder = new PulseBusBuilder(services);
        configure(builder);
        
        services.AddSingleton(builder.Options);

        services.AddSingleton<IMessageBus>(sp =>
        {
            var options = sp.GetRequiredService<BusOptions>();
            return new DefaultMessageBus(options);
        });

        builder.Options.Serializer ??= new JsonMessageSerializer();

        return services;
    }

    private class JsonMessageSerializer : IMessageSerializer
    {
        public byte[] Serialize<T>(T message)
            => JsonSerializer.SerializeToUtf8Bytes(message);

        public T Deserialize<T>(byte[] data)
            => JsonSerializer.Deserialize<T>(data);
    }
}
