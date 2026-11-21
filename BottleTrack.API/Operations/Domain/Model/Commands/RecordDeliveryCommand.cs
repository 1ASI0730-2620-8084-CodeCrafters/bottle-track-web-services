using BottleTrack.API.Operations.Domain.Model.ValueObjects;

namespace BottleTrack.API.Operations.Domain.Model.Commands;

public class RecordDeliveryCommand
{
    public Guid TransportOperationId { get; }

    public Guid DeliveryStopId { get; }

    public DeliveryStopStatus Status { get; }

    public int DeliveredQuantity { get; }

    public string? FailureReason { get; }

    public RecordDeliveryCommand(Guid transportOperationId, Guid deliveryStopId, DeliveryStopStatus status, int deliveredQuantity, string? failureReason)
    {
        TransportOperationId = transportOperationId;
        DeliveryStopId = deliveryStopId;
        Status = status;
        DeliveredQuantity = deliveredQuantity;
        FailureReason = failureReason;
    }
}
