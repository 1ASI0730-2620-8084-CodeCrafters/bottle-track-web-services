namespace BottleTrack.API.Operations.Domain.Model.Queries;

public class GetDeliveryPointByIdQuery
{
    public Guid DeliveryPointId { get; }

    public GetDeliveryPointByIdQuery(Guid deliveryPointId)
    {
        DeliveryPointId = deliveryPointId;
    }
}
