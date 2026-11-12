using BottleTrack.API.Operations.Domain.Model.ValueObjects;

namespace BottleTrack.API.Operations.Interfaces.Rest.Resources;

public class DeliveryStopResource
{
    public Guid Id { get; set; }

    public Guid DeliveryPointId { get; set; }

    public int SequenceNumber { get; set; }

    public int PlannedQuantity { get; set; }

    public int? DeliveredQuantity { get; set; }

    public DeliveryStopStatus Status { get; set; }

    public string? FailureReason { get; set; }

    public DateTime? ConfirmedAt { get; set; }
}
