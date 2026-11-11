using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Repositories;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BottleTrack.API.Operations.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class TransportOperationRepository : BaseRepository<TransportOperation>, ITransportOperationRepository
{
    public TransportOperationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<TransportOperation?> FindByIdWithStopsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TransportOperation>()
            .Include(operation => operation.DeliveryStops)
            .FirstOrDefaultAsync(operation => operation.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<TransportOperation>> ListByOrganizationAsync(Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<TransportOperation>()
            .Include(operation => operation.DeliveryStops)
            .Where(operation => operation.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TransportOperation>().CountAsync(cancellationToken);
    }
}
