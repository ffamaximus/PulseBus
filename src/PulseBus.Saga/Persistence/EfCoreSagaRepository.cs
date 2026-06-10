using PulseBus.Saga.Abstractions;

namespace PulseBus.Saga.Persistence;

public class EfCoreSagaRepository : ISagaRepository
{
    private readonly SagaDbContext _db;

    public EfCoreSagaRepository(SagaDbContext db)
    {
        _db = db;
    }

    public async Task<ISagaState?> GetAsync(Guid id, Type stateType, CancellationToken ct)
    {
        return await _db.SagaStates.FindAsync(new object[] { id }, ct);
    }

    public async Task SaveAsync(ISagaState state, CancellationToken ct)
    {
        var entity = (SagaStateEntity)state;

        _db.SagaStates.Update(entity);
        await _db.SaveChangesAsync(ct);
    }
}
