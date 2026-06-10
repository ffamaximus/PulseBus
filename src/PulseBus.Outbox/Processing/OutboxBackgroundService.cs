using Microsoft.Extensions.Hosting;
using PulseBus.Outbox.Abstractions;

namespace PulseBus.Outbox.Processing;

public class OutboxBackgroundService(IOutboxProcessor processor) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await processor.ProcessAsync(ct);
            await Task.Delay(500, ct);
        }
    }
}