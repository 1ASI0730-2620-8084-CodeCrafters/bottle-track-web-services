using BottleTrack.API.Fleet.Interfaces.ACL;

namespace BottleTrack.API.Operations.Application.Internal.OutboundServices;

public class ExternalFleetService
{
    private readonly IFleetContextFacade fleetContextFacade;

    public ExternalFleetService(IFleetContextFacade fleetContextFacade)
    {
        this.fleetContextFacade = fleetContextFacade;
    }

    public async Task<bool> SwapVehicleAsync(Guid? currentVehicleId, Guid newVehicleId,
        CancellationToken cancellationToken = default)
    {
        if (currentVehicleId == newVehicleId)
            return true;

        bool isReserved = await fleetContextFacade.ReserveVehicleAsync(newVehicleId, cancellationToken);
        if (!isReserved)
            return false;

        if (currentVehicleId.HasValue)
            await fleetContextFacade.ReleaseVehicleAsync(currentVehicleId.Value, cancellationToken);

        return true;
    }

    public async Task<bool> SwapDriverAsync(Guid? currentDriverId, Guid newDriverId,
        CancellationToken cancellationToken = default)
    {
        if (currentDriverId == newDriverId)
            return true;

        bool isReserved = await fleetContextFacade.ReserveDriverAsync(newDriverId, cancellationToken);
        if (!isReserved)
            return false;

        if (currentDriverId.HasValue)
            await fleetContextFacade.ReleaseDriverAsync(currentDriverId.Value, cancellationToken);

        return true;
    }

    public async Task ReleaseResourcesAsync(Guid? vehicleId, Guid? driverId,
        CancellationToken cancellationToken = default)
    {
        if (vehicleId.HasValue)
            await fleetContextFacade.ReleaseVehicleAsync(vehicleId.Value, cancellationToken);

        if (driverId.HasValue)
            await fleetContextFacade.ReleaseDriverAsync(driverId.Value, cancellationToken);
    }
}
