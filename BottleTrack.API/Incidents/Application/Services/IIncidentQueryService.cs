using BottleTrack.API.Incidents.Domain.Model.Aggregates;
using BottleTrack.API.Incidents.Domain.Model.Queries;

namespace BottleTrack.API.Incidents.Application.Services;

public interface IIncidentQueryService
{
    Task<Incident?> Handle(GetIncidentByIdQuery query, CancellationToken cancellationToken = default);

    Task<IEnumerable<Incident>> Handle(GetIncidentsByTransportOperationIdQuery query,
        CancellationToken cancellationToken = default);
}
