namespace BottleTrack.API.Incidents.Domain.Model.Queries;

public class GetIncidentsByTransportOperationIdQuery
{
    public Guid TransportOperationId { get; }

    public GetIncidentsByTransportOperationIdQuery(Guid transportOperationId)
    {
        TransportOperationId = transportOperationId;
    }
}
