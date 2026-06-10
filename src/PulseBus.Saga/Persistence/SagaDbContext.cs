using Microsoft.EntityFrameworkCore;

namespace PulseBus.Saga.Persistence;

public class SagaDbContext(DbContextOptions<SagaDbContext> options) : DbContext(options)
{
    public DbSet<SagaStateEntity> SagaStates => Set<SagaStateEntity>();
}