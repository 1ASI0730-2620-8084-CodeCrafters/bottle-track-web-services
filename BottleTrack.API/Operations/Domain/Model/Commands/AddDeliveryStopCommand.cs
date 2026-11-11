namespace BottleTrack.API.Operations.Domain.Model.Commands;

public class AddDeliveryStopCommand
{
    public Guid TransportOperationId { get; }

    public Guid DeliveryPointId { get; }

    public int SequenceNumber { get; }

    public int PlannedQuantity { get; }

    public AddDeliveryStopCommand(Guid transportOperationId, Guid deliveryPointId, int sequenceNumber,
        int plannedQuantity)
    {
        TransportOperationId = transportOperationId;
        DeliveryPointId = deliveryPointId;
        SequenceNumber = sequenceNumber;
        PlannedQuantity = plannedQuantity;
    }
}
