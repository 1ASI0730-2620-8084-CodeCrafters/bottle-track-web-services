using BottleTrack.API.Incidents.Application.Services;
using BottleTrack.API.Incidents.Domain.Model.Aggregates;
using BottleTrack.API.Incidents.Domain.Model.Queries;
using BottleTrack.API.Incidents.Domain.Repositories;

namespace BottleTrack.API.Incidents.Application.Internal.QueryServices;

public class IncidentQueryService : IIncidentQueryService
{
    private readonly IIncidentRepository incidentRepository;

    public IncidentQueryService(IIncidentRepository incidentRepository)
    {
        this.incidentRepository = incidentRepository;
    }

    public async Task<Incident?> Handle(GetIncidentByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await incidentRepository.FindByIdWithEvidencesAsync(query.IncidentId, cancellationToken);
    }

    public async Task<IEnumerable<Incident>> Handle(GetIncidentsByTransportOperationIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await incidentRepository.ListByTransportOperationAsync(query.TransportOperationId, cancellationToken);
    }
}
