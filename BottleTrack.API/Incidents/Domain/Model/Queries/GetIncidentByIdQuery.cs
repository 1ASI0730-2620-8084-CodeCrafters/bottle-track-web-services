namespace BottleTrack.API.Incidents.Domain.Model.Queries;

public class GetIncidentByIdQuery
{
    public Guid IncidentId { get; }

    public GetIncidentByIdQuery(Guid incidentId)
    {
        IncidentId = incidentId;
    }
}
