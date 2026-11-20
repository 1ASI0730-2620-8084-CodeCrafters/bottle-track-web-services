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

    public bool CanChangeResources
    {
        get { return Status == TransportOperationStatus.Pending; }
    }

    public bool CanStart
    {
        get
        {
            bool isWaiting = Status == TransportOperationStatus.Pending ||
                             Status == TransportOperationStatus.Dispatched;
            return isWaiting && VehicleId.HasValue && DriverId.HasValue && DeliveryStops.Count > 0;
        }
    }

    public bool CanRecordDeliveries
    {
        get { return Status == TransportOperationStatus.InProgress; }
    }

    public bool CanComplete
    {
        get
        {
            if (Status != TransportOperationStatus.InProgress || DeliveryStops.Count == 0)
                return false;

            foreach (DeliveryStop deliveryStop in DeliveryStops)
            {
                if (deliveryStop.Status == DeliveryStopStatus.Pending)
                    return false;
            }

            return true;
        }
    }

    public bool CanCancel
    {
        get
        {
            return Status == TransportOperationStatus.Pending || Status == TransportOperationStatus.Dispatched;
        }
    }

    public void AddStop(Guid deliveryPointId, int sequenceNumber, int plannedQuantity)
    {
        if (!CanChangeResources)
            throw new InvalidOperationException("Stops can only be added to a pending operation.");

        DeliveryStop deliveryStop = DeliveryStop.Create(Id, deliveryPointId, sequenceNumber, plannedQuantity);
        DeliveryStops.Add(deliveryStop);
    }

    public bool CanMoveStop(Guid deliveryStopId, int offset)
    {
        if (!CanChangeResources || (offset != -1 && offset != 1))
            return false;

        DeliveryStop? deliveryStop = FindStop(deliveryStopId);
        if (deliveryStop == null)
            return false;

        return FindStopBySequence(deliveryStop.SequenceNumber + offset) != null;
    }

    public void MoveStop(Guid deliveryStopId, int offset)
    {
        if (!CanMoveStop(deliveryStopId, offset))
            throw new InvalidOperationException("The stop cannot be moved to that position.");

        DeliveryStop deliveryStop = FindStop(deliveryStopId)!;
        int currentSequence = deliveryStop.SequenceNumber;
        DeliveryStop neighbour = FindStopBySequence(currentSequence + offset)!;

        neighbour.ChangeSequence(currentSequence);
        deliveryStop.ChangeSequence(currentSequence + offset);
    }

    public void AssignVehicle(Guid vehicleId)
    {
        if (!CanChangeResources)
            throw new InvalidOperationException("A vehicle can only be assigned to a pending operation.");

        VehicleId = vehicleId;
    }

    public void AssignDriver(Guid driverId)
    {
        if (!CanChangeResources)
            throw new InvalidOperationException("A driver can only be assigned to a pending operation.");

        DriverId = driverId;
    }

    public void Dispatch(Guid vehicleId, Guid driverId)
    {
        AssignVehicle(vehicleId);
        AssignDriver(driverId);
        Status = TransportOperationStatus.Dispatched;
        DispatchedAt = DateTime.UtcNow;
    }

    public void Start()
    {
        if (!CanStart)
            throw new InvalidOperationException("The operation needs a vehicle, a driver and stops to start.");

        Status = TransportOperationStatus.InProgress;
        if (!DispatchedAt.HasValue)
            DispatchedAt = DateTime.UtcNow;
    }

    public DeliveryStop? FindStop(Guid deliveryStopId)
    {
        foreach (DeliveryStop deliveryStop in DeliveryStops)
        {
            if (deliveryStop.Id == deliveryStopId)
                return deliveryStop;
        }

        return null;
    }

    private DeliveryStop? FindStopBySequence(int sequenceNumber)
    {
        foreach (DeliveryStop deliveryStop in DeliveryStops)
        {
            if (deliveryStop.SequenceNumber == sequenceNumber)
                return deliveryStop;
        }

        return null;
    }

    public void RecordDelivery(Guid deliveryStopId, DeliveryStopStatus status, int deliveredQuantity,
        string? failureReason)
    {
        if (!CanRecordDeliveries)
            throw new InvalidOperationException("Deliveries can only be recorded on an operation in progress.");

        DeliveryStop? deliveryStop = FindStop(deliveryStopId);
        if (deliveryStop == null)
            throw new InvalidOperationException("The delivery stop does not belong to this operation.");

        deliveryStop.Record(status, deliveredQuantity, failureReason);
    }

    public void Complete()
    {
        if (!CanComplete)
            throw new InvalidOperationException("Every stop needs a result before completing the operation.");

        Status = TransportOperationStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void Cancel(string reason)
    {
        if (!CanCancel)
            throw new InvalidOperationException("Only a pending or dispatched operation can be cancelled.");

        Status = TransportOperationStatus.Cancelled;
        CancellationReason = reason.Trim();
    }
}
