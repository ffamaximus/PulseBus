using PulseBus.Outbox.Abstractions;

namespace PulseBus.Outbox.Processing;

public class OutboxProcessor(
    IOutboxStore store,
    IOutboxSerializer serializer,
    IOutboxPublisher publisher)
    : IOutboxProcessor
{
    public async Task ProcessAsync(CancellationToken ct)
    {
        var messages = await store.GetPendingAsync(50, ct);

        foreach (var msg in messages)
        {
            var evt = serializer.Deserialize(msg.Payload, msg.Type);

            await publisher.PublishAsync(evt, ct);

            await store.MarkAsProcessedAsync(msg.Id, ct);
        }
    }
}