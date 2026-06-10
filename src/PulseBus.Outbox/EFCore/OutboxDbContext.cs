using Microsoft.EntityFrameworkCore;

namespace PulseBus.Outbox.EFCore;

public class OutboxDbContext(DbContextOptions<OutboxDbContext> options) : DbContext(options)
{
	public DbSet<OutboxMessageEntity> OutboxMessages => Set<OutboxMessageEntity>();
}