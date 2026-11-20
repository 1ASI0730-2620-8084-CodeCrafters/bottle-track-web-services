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

    public void ChangeSequence(int sequenceNumber)
    {
        SequenceNumber = sequenceNumber;
    }

    public bool IsValidResult(DeliveryStopStatus status, int deliveredQuantity, string? failureReason)
    {
        if (Status != DeliveryStopStatus.Pending)
            return false;

        if (status == DeliveryStopStatus.Delivered)
            return true;

        if (string.IsNullOrWhiteSpace(failureReason))
            return false;

        if (status == DeliveryStopStatus.NotDelivered)
            return true;

        if (status == DeliveryStopStatus.PartiallyDelivered)
            return deliveredQuantity > 0 && deliveredQuantity < PlannedQuantity;

        return false;
    }

    public void Record(DeliveryStopStatus status, int deliveredQuantity, string? failureReason)
    {
        if (!IsValidResult(status, deliveredQuantity, failureReason))
            throw new InvalidOperationException("The delivery result is not valid for this stop.");

        Status = status;
        DeliveredQuantity = deliveredQuantity;
        FailureReason = null;

        if (status == DeliveryStopStatus.Delivered)
            DeliveredQuantity = PlannedQuantity;
        else
            FailureReason = failureReason!.Trim();

        if (status == DeliveryStopStatus.NotDelivered)
            DeliveredQuantity = 0;

        ConfirmedAt = DateTime.UtcNow;
    }
}
