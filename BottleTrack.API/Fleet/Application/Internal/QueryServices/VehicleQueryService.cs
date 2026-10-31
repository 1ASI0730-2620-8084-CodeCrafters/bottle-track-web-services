using BottleTrack.API.Fleet.Application.Services;
using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.Queries;
using BottleTrack.API.Fleet.Domain.Repositories;

namespace BottleTrack.API.Fleet.Application.Internal.QueryServices;

public class VehicleQueryService : IVehicleQueryService
{
    private readonly IVehicleRepository vehicleRepository;

    public VehicleQueryService(IVehicleRepository vehicleRepository)
    {
        this.vehicleRepository = vehicleRepository;
    }

    public async Task<Vehicle?> Handle(GetVehicleByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await vehicleRepository.FindByIdAsync(query.VehicleId, cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> Handle(GetVehiclesByOrganizationIdQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.OnlyAvailable)
            return await vehicleRepository.ListAvailableAsync(query.OrganizationId, cancellationToken);

        return await vehicleRepository.ListByOrganizationAsync(query.OrganizationId, cancellationToken);
    }
}
