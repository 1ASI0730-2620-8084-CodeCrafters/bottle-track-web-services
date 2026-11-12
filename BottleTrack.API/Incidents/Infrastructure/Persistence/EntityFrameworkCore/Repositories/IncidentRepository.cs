using BottleTrack.API.Incidents.Domain.Model.Aggregates;
using BottleTrack.API.Incidents.Domain.Repositories;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BottleTrack.API.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class IncidentRepository : BaseRepository<Incident>, IIncidentRepository
{
    public IncidentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Incident?> FindByIdWithEvidencesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Incident>()
            .Include(incident => incident.Evidences)
            .FirstOrDefaultAsync(incident => incident.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Incident>> ListByTransportOperationAsync(Guid transportOperationId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Incident>()
            .Include(incident => incident.Evidences)
            .Where(incident => incident.TransportOperationId == transportOperationId)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Incident>().CountAsync(cancellationToken);
    }
}
