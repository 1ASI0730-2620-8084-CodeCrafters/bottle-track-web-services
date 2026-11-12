using BottleTrack.API.Operations.Domain.Model.Aggregates;
using BottleTrack.API.Operations.Domain.Model.Commands;
using BottleTrack.API.Operations.Domain.Model.Entities;
using BottleTrack.API.Operations.Interfaces.Rest.Resources;

namespace BottleTrack.API.Operations.Interfaces.Rest.Transform;

public static class TransportOperationAssembler
{
    public static CreateTransportOperationCommand ToCommand(CreateTransportOperationResource resource)
    {
        return new CreateTransportOperationCommand(resource.OrganizationId, resource.Description,
            resource.ScheduledDate);
    }

    public static TransportOperationResource ToResource(TransportOperation operation)
    {
        TransportOperationResource resource = new TransportOperationResource();
        resource.Id = operation.Id;
        resource.OrganizationId = operation.OrganizationId;
        resource.Code = operation.Code;
        resource.Description = operation.Description;
        resource.ScheduledDate = operation.ScheduledDate;
        resource.VehicleId = operation.VehicleId;
        resource.DriverId = operation.DriverId;
        resource.Status = operation.Status;
        resource.DispatchedAt = operation.DispatchedAt;
        resource.CompletedAt = operation.CompletedAt;
        resource.CancellationReason = operation.CancellationReason;

        foreach (DeliveryStop deliveryStop in operation.DeliveryStops)
        {
            DeliveryStopResource deliveryStopResource = new DeliveryStopResource();
            deliveryStopResource.Id = deliveryStop.Id;
            deliveryStopResource.DeliveryPointId = deliveryStop.DeliveryPointId;
            deliveryStopResource.SequenceNumber = deliveryStop.SequenceNumber;
            deliveryStopResource.PlannedQuantity = deliveryStop.PlannedQuantity;
            deliveryStopResource.DeliveredQuantity = deliveryStop.DeliveredQuantity;
            deliveryStopResource.Status = deliveryStop.Status;
            deliveryStopResource.FailureReason = deliveryStop.FailureReason;
            deliveryStopResource.ConfirmedAt = deliveryStop.ConfirmedAt;
            resource.DeliveryStops.Add(deliveryStopResource);
        }

        return resource;
    }
}
