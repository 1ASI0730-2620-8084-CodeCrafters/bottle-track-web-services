namespace BottleTrack.API.Operations.Domain.Model.Commands;

public class AssignDriverCommand
{
    public Guid TransportOperationId { get; }

    public Guid DriverId { get; }

    public AssignDriverCommand(Guid transportOperationId, Guid driverId)
    {
        TransportOperationId = transportOperationId;
        DriverId = driverId;
    }
}
