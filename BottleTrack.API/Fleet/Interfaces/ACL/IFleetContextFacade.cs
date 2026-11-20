namespace BottleTrack.API.Fleet.Interfaces.ACL;

public interface IFleetContextFacade
{
    Task<bool> ReserveVehicleAsync(Guid vehicleId, CancellationToken cancellationToken = default);

    Task ReleaseVehicleAsync(Guid vehicleId, CancellationToken cancellationToken = default);

    Task<bool> ReserveDriverAsync(Guid driverId, CancellationToken cancellationToken = default);

    Task ReleaseDriverAsync(Guid driverId, CancellationToken cancellationToken = default);
}
