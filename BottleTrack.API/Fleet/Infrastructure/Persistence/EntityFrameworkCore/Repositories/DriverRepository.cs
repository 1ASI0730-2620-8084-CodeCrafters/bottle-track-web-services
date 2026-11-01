using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.ValueObjects;
using BottleTrack.API.Fleet.Domain.Repositories;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BottleTrack.API.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class DriverRepository : BaseRepository<Driver>, IDriverRepository
{
    public DriverRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Driver?> FindByDocumentNumberAsync(string documentNumber,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Driver>()
            .FirstOrDefaultAsync(driver => driver.DocumentNumber == documentNumber, cancellationToken);
    }

    public async Task<Driver?> FindByLicenseNumberAsync(LicenseNumber licenseNumber,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Driver>()
            .FirstOrDefaultAsync(driver => driver.LicenseNumber == licenseNumber, cancellationToken);
    }

    public async Task<IEnumerable<Driver>> ListByOrganizationAsync(Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Driver>()
            .Where(driver => driver.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Driver>> ListAvailableAsync(Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Driver>()
            .Where(driver => driver.OrganizationId == organizationId && driver.Status == DriverStatus.Available)
            .ToListAsync(cancellationToken);
    }
}
