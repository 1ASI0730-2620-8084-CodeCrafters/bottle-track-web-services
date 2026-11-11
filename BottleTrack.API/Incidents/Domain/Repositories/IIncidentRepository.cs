using BottleTrack.API.Incidents.Domain.Model.Aggregates;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Incidents.Domain.Repositories;

public interface IIncidentRepository : IBaseRepository<Incident>
{
    Task<Incident?> FindByIdWithEvidencesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Incident>> ListByTransportOperationAsync(Guid transportOperationId,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
}
