using BottleTrack.API.Fleet.Domain.Model.Aggregates;
using BottleTrack.API.Fleet.Domain.Model.ValueObjects;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Fleet.Domain.Repositories;

public interface IDriverRepository : IBaseRepository<Driver>
{
    Task<Driver?> FindByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default);

    Task<Driver?> FindByLicenseNumberAsync(LicenseNumber licenseNumber, CancellationToken cancellationToken = default);

    Task<IEnumerable<Driver>> ListByOrganizationAsync(Guid organizationId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Driver>> ListAvailableAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
