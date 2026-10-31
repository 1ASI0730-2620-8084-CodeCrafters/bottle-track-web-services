namespace BottleTrack.API.Fleet.Domain.Model.Queries;

public class GetVehicleByIdQuery
{
    public Guid VehicleId { get; }

    public GetVehicleByIdQuery(Guid vehicleId)
    {
        VehicleId = vehicleId;
    }
}
