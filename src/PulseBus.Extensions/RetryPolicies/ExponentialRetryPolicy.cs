using System;
using PulseBus.Abstractions;
using PulseBus.Models;

namespace PulseBus.Extensions.RetryPolicies;

public class ExponentialRetryPolicy : IMessageRetryPolicy
{
    public int MaxRetries { get; }

    public ExponentialRetryPolicy(int maxRetries = 5)
    {
        MaxRetries = maxRetries;
    }

    public bool ShouldRetry(RetryContext context)
        => context.Attempt < MaxRetries;

    public TimeSpan GetDelay(RetryContext context)
        => TimeSpan.FromSeconds(Math.Pow(2, context.Attempt));
}