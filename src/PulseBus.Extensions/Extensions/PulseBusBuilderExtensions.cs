using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PulseBus.Builders;
using PulseBus.Extensions.Idempotency;
using PulseBus.Extensions.Middlewares;
using PulseBus.Extensions.RetryPolicies;
using PulseBus.Extensions.Serialization;

namespace PulseBus.Extensions.Extensions;

public static class PulseBusBuilderExtensions
{
    public static PulseBusBuilder AddDefaultExtensions(this PulseBusBuilder builder)
    {
        builder.UseJsonSerializer();
        builder.UseExponentialRetry();
        builder.UseInMemoryIdempotency();
        builder.UseLogging();
        return builder;
    }
    
    public static void UseJsonSerializer(this PulseBusBuilder builder)
    {
        builder.Options.Serializer = new JsonMessageSerializer();
    }
    
    public static void UseExponentialRetry(this PulseBusBuilder builder, int maxRetries = 5)
    {
        builder.Options.RetryPolicy = new ExponentialRetryPolicy(maxRetries);
    }
    
    public static void UseLogging(this PulseBusBuilder builder)
    {
        builder.Options.Middlewares.Add(new LoggingMiddleware(
            builder.Services.BuildServiceProvider()
                .GetRequiredService<ILogger<LoggingMiddleware>>()));
    }
    
    public static void UseInMemoryIdempotency(this PulseBusBuilder builder)
    {
        builder.Options.IdempotencyStore = new InMemoryIdempotencyStore();
    }
}