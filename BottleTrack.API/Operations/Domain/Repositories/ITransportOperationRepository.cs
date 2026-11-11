using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Operations.Domain.Repositories;

public interface ITransportOperationRepository : IBaseRepository<TransportOperation>
{
    Task<TransportOperation?> FindByIdWithStopsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<TransportOperation>> ListByOrganizationAsync(Guid organizationId,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
}
