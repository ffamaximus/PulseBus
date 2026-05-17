using PulseBus.Core.Models;

namespace PulseBus.Core.Abstractions;

public interface IMessageRetryPolicy
{
    bool ShouldRetry(RetryContext context);
    TimeSpan GetDelay(RetryContext context);
}