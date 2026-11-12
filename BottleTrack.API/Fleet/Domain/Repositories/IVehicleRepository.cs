using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.ValueObjects;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Fleet.Domain.Repositories;

public interface IVehicleRepository : IBaseRepository<Vehicle>
{
    Task<Vehicle?> FindByPlateNumberAsync(PlateNumber plateNumber, CancellationToken cancellationToken = default);

    Task<IEnumerable<Vehicle>> ListByOrganizationAsync(Guid organizationId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Vehicle>> ListAvailableAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
