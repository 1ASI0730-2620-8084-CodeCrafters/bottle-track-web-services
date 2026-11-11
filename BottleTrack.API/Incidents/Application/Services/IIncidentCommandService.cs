using BottleTrack.API.Incidents.Application.Errors;
using BottleTrack.API.Incidents.Domain.Model.Aggregates;
using BottleTrack.API.Incidents.Domain.Model.Commands;
using BottleTrack.API.Shared.Application.Patterns;

namespace BottleTrack.API.Incidents.Application.Services;

public interface IIncidentCommandService
{
    Task<Result<Incident, IncidentError>> Handle(ReportIncidentCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<Incident, IncidentError>> Handle(AddEvidenceCommand command,
        CancellationToken cancellationToken = default);

    Task<Result<Incident, IncidentError>> Handle(ResolveIncidentCommand command,
        CancellationToken cancellationToken = default);
}
