using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Iam.Domain.Repositories;

public interface IOrganizationRepository : IBaseRepository<Organization>
{
    Task<Organization?> FindByTaxIdAsync(TaxId taxId, CancellationToken cancellationToken = default);
}
