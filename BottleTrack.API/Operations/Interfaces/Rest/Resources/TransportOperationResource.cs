using BottleTrack.API.Operations.Domain.Model.ValueObjects;

namespace BottleTrack.API.Operations.Interfaces.Rest.Resources;

public class TransportOperationResource
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateOnly ScheduledDate { get; set; }

    public Guid? VehicleId { get; set; }

    public Guid? DriverId { get; set; }

    public TransportOperationStatus Status { get; set; }

    public DateTime? DispatchedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string? CancellationReason { get; set; }

    public List<DeliveryStopResource> DeliveryStops { get; set; } = new List<DeliveryStopResource>();
}
