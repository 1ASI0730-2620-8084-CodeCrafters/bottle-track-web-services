namespace BottleTrack.API.Operations.Domain.Model.Queries;

public class GetTransportOperationByIdQuery
{
    public Guid TransportOperationId { get; }

    public GetTransportOperationByIdQuery(Guid transportOperationId)
    {
        TransportOperationId = transportOperationId;
    }
}
