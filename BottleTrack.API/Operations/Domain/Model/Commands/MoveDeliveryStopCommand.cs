namespace BottleTrack.API.Operations.Domain.Model.Commands;

public class MoveDeliveryStopCommand
{
    public Guid TransportOperationId { get; }

    public Guid DeliveryStopId { get; }

    public int Offset { get; }

    public MoveDeliveryStopCommand(Guid transportOperationId, Guid deliveryStopId, int offset)
    {
        TransportOperationId = transportOperationId;
        DeliveryStopId = deliveryStopId;
        Offset = offset;
    }
}
