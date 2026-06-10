using Microsoft.EntityFrameworkCore;
using PulseBus.Outbox.Abstractions;

namespace PulseBus.Outbox.EFCore;

public class EfCoreOutboxStore(OutboxDbContext db) : IOutboxStore
{
	public async Task AddAsync(IOutboxMessage message, CancellationToken ct)
	{
		db.OutboxMessages.Add((OutboxMessageEntity)message);
		await db.SaveChangesAsync(ct);
	}

	public async Task<IReadOnlyList<IOutboxMessage>> GetPendingAsync(int max, CancellationToken ct)
	{
		return await db.OutboxMessages
			.Where(x => x.ProcessedAt == null)
			.OrderBy(x => x.CreatedAt)
			.Take(max)
			.ToListAsync(ct);
	}

	public async Task MarkAsProcessedAsync(Guid id, CancellationToken ct)
	{
		var msg = await db.OutboxMessages.FindAsync(new object[] { id }, ct);
		if (msg != null)
		{
			msg.ProcessedAt = DateTime.UtcNow;
			await db.SaveChangesAsync(ct);
		}
	}
}