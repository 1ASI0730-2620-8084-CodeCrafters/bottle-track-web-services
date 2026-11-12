using BottleTrack.API.Operations.Domain.Model.Entities;
using BottleTrack.API.Operations.Domain.Model.ValueObjects;

namespace BottleTrack.API.Operations.Domain.Model.Aggregates;

public class TransportOperation
{
    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public string Code { get; private set; }

    public string Description { get; private set; }

    public DateOnly ScheduledDate { get; private set; }

    public Guid? VehicleId { get; private set; }

    public Guid? DriverId { get; private set; }

    public TransportOperationStatus Status { get; private set; }

    public DateTime? DispatchedAt { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public string? CancellationReason { get; private set; }

    public List<DeliveryStop> DeliveryStops { get; private set; }

    private TransportOperation()
    {
        Code = string.Empty;
        Description = string.Empty;
        DeliveryStops = new List<DeliveryStop>();
    }

    public static TransportOperation Create(string code, Guid organizationId, string description,
        DateOnly scheduledDate)
    {
        TransportOperation operation = new TransportOperation();
        operation.Id = Guid.NewGuid();
        operation.Code = code;
        operation.OrganizationId = organizationId;
        operation.Description = description.Trim();
        operation.ScheduledDate = scheduledDate;
        operation.Status = TransportOperationStatus.Pending;
        return operation;
    }

    public void AddStop(Guid deliveryPointId, int sequenceNumber, int plannedQuantity)
    {
        DeliveryStop deliveryStop = DeliveryStop.Create(Id, deliveryPointId, sequenceNumber, plannedQuantity);
        DeliveryStops.Add(deliveryStop);
    }

    public void Dispatch(Guid vehicleId, Guid driverId)
    {
        VehicleId = vehicleId;
        DriverId = driverId;
        Status = TransportOperationStatus.Dispatched;
        DispatchedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = TransportOperationStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void Cancel(string reason)
    {
        Status = TransportOperationStatus.Cancelled;
        CancellationReason = reason.Trim();
    }
}
