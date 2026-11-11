namespace BottleTrack.API.Operations.Domain.Model.Commands;

public class DispatchTransportOperationCommand
{
    public Guid TransportOperationId { get; }

    public Guid VehicleId { get; }

    public Guid DriverId { get; }

    public DispatchTransportOperationCommand(Guid transportOperationId, Guid vehicleId, Guid driverId)
    {
        TransportOperationId = transportOperationId;
        VehicleId = vehicleId;
        DriverId = driverId;
    }
}
