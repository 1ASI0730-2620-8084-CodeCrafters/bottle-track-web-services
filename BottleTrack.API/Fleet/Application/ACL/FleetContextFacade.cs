using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.ValueObjects;
using BottleTrack.API.Fleet.Domain.Repositories;
using BottleTrack.API.Fleet.Interfaces.ACL;

namespace BottleTrack.API.Fleet.Application.ACL;

public class FleetContextFacade : IFleetContextFacade
{
    private readonly IVehicleRepository vehicleRepository;
    private readonly IDriverRepository driverRepository;

    public FleetContextFacade(IVehicleRepository vehicleRepository, IDriverRepository driverRepository)
    {
        this.vehicleRepository = vehicleRepository;
        this.driverRepository = driverRepository;
    }

    public async Task<bool> ReserveVehicleAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        Vehicle? vehicle = await vehicleRepository.FindByIdAsync(vehicleId, cancellationToken);
        if (vehicle == null || vehicle.Status != VehicleStatus.Available)
            return false;

        vehicle.Reserve();
        vehicleRepository.Update(vehicle);
        return true;
    }

    public async Task ReleaseVehicleAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        Vehicle? vehicle = await vehicleRepository.FindByIdAsync(vehicleId, cancellationToken);
        if (vehicle == null || vehicle.Status != VehicleStatus.Assigned)
            return;

        vehicle.Release();
        vehicleRepository.Update(vehicle);
    }

    public async Task<bool> ReserveDriverAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        Driver? driver = await driverRepository.FindByIdAsync(driverId, cancellationToken);
        if (driver == null || driver.Status != DriverStatus.Available)
            return false;

        driver.Reserve();
        driverRepository.Update(driver);
        return true;
    }

    public async Task ReleaseDriverAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        Driver? driver = await driverRepository.FindByIdAsync(driverId, cancellationToken);
        if (driver == null || driver.Status != DriverStatus.Assigned)
            return;

        driver.Release();
        driverRepository.Update(driver);
    }
}
