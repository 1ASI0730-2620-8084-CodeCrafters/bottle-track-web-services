using BottleTrack.API.Operations.Domain.Model.ValueObjects;

namespace BottleTrack.API.Operations.Domain.Model.Entities;

public class DeliveryStop
{
    public Guid Id { get; private set; }

    public Guid TransportOperationId { get; private set; }

    public Guid DeliveryPointId { get; private set; }

    public int SequenceNumber { get; private set; }

    public int PlannedQuantity { get; private set; }

    public int? DeliveredQuantity { get; private set; }

    public DeliveryStopStatus Status { get; private set; }

    public string? FailureReason { get; private set; }

    public DateTime? ConfirmedAt { get; private set; }

    private DeliveryStop()
    {
    }

    public static DeliveryStop Create(Guid transportOperationId, Guid deliveryPointId, int sequenceNumber,
        int plannedQuantity)
    {
        DeliveryStop deliveryStop = new DeliveryStop();
        deliveryStop.Id = Guid.NewGuid();
        deliveryStop.TransportOperationId = transportOperationId;
        deliveryStop.DeliveryPointId = deliveryPointId;
        deliveryStop.SequenceNumber = sequenceNumber;
        deliveryStop.PlannedQuantity = plannedQuantity;
        deliveryStop.Status = DeliveryStopStatus.Pending;
        return deliveryStop;
    }
}
