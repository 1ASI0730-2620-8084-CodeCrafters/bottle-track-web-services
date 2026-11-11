using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Repositories;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BottleTrack.API.Operations.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class DeliveryPointRepository : BaseRepository<DeliveryPoint>, IDeliveryPointRepository
{
    public DeliveryPointRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DeliveryPoint>> ListByOrganizationAsync(Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<DeliveryPoint>()
            .Where(deliveryPoint => deliveryPoint.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }
}
