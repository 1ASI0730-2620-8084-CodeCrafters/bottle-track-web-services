namespace BottleTrack.API.Fleet.Domain.Model.Queries;

public class GetDriverByIdQuery
{
    public Guid DriverId { get; }

    public GetDriverByIdQuery(Guid driverId)
    {
        DriverId = driverId;
    }
}
