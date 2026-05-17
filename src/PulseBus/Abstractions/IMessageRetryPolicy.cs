using System;
using PulseBus.Models;

namespace PulseBus.Abstractions;

public interface IMessageRetryPolicy
{
    bool ShouldRetry(RetryContext context);
    TimeSpan GetDelay(RetryContext context);
}