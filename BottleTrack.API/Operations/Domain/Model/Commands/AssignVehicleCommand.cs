namespace BottleTrack.API.Operations.Domain.Model.Commands;

public class AssignVehicleCommand
{
    public Guid TransportOperationId { get; }

    public Guid VehicleId { get; }

    public AssignVehicleCommand(Guid transportOperationId, Guid vehicleId)
    {
        TransportOperationId = transportOperationId;
        VehicleId = vehicleId;
    }
}
