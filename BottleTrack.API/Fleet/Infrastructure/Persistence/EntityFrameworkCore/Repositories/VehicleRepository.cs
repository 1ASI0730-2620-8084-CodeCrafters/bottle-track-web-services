using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.ValueObjects;
using BottleTrack.API.Fleet.Domain.Repositories;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BottleTrack.API.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Vehicle?> FindByPlateNumberAsync(PlateNumber plateNumber,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .FirstOrDefaultAsync(vehicle => vehicle.PlateNumber == plateNumber, cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> ListByOrganizationAsync(Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .Where(vehicle => vehicle.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> ListAvailableAsync(Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .Where(vehicle => vehicle.OrganizationId == organizationId && vehicle.Status == VehicleStatus.Available)
            .ToListAsync(cancellationToken);
    }
}
