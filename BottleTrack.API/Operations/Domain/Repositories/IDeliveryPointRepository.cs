using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Operations.Domain.Repositories;

public interface IDeliveryPointRepository : IBaseRepository<DeliveryPoint>
{
    Task<IEnumerable<DeliveryPoint>> ListByOrganizationAsync(Guid organizationId,
        CancellationToken cancellationToken = default);
}
