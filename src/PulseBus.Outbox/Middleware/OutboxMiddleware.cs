using PulseBus.Abstractions;
using PulseBus.Outbox.Abstractions;
using PulseBus.Outbox.EFCore;
using PulseBus.Pipeline;

namespace PulseBus.Outbox.Middleware;

public class OutboxMiddleware(IOutboxStore store, IOutboxSerializer serializer) : IMessageMiddleware
{
    public async Task InvokeAsync(MiddlewareContext ctx, MiddlewareDelegate next)
    {
        if (ctx is not MessageContext context)
            throw new InvalidOperationException("OutboxMiddleware requires MessageContext");

        var evt = context.Message;

        var msg = new OutboxMessageEntity
        {
            Id = Guid.NewGuid(),
            Type = evt.GetType().FullName!,
            Payload = serializer.Serialize(evt)
        };

        await store.AddAsync(msg, context.CancellationToken);

        await next(context);
    }

}
