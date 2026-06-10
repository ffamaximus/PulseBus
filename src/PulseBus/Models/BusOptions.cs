using System.Collections.Generic;
using PulseBus.Abstractions;

namespace PulseBus.Models;

public record BusOptions
{
    public IBusProvider Provider { get; set; }
    public IMessageSerializer Serializer { get; set; }
    public IList<IMessageMiddleware> Middlewares { get; set; } = new List<IMessageMiddleware>();
    public IMessageRetryPolicy RetryPolicy { get; set; }
    public IIdempotencyStore IdempotencyStore { get; set; }
}