using Microsoft.Extensions.DependencyInjection;
using PulseBus.Outbox.Abstractions;
using PulseBus.Outbox.EFCore;
using PulseBus.Outbox.Processing;

namespace PulseBus.Outbox.Extensions;

public static class OutboxServiceCollectionExtensions
{
    public static IServiceCollection AddPulseBusOutbox(
        this IServiceCollection services,
        Action<OutboxOptions>? configure = null)
    {
        services.AddScoped<IOutboxStore, EfCoreOutboxStore>();
        services.AddSingleton<IOutboxSerializer, JsonOutboxSerializer>();
        services.AddScoped<IOutboxProcessor, OutboxProcessor>();
        services.AddHostedService<OutboxBackgroundService>();

        configure?.Invoke(new OutboxOptions());

        return services;
    }
}