using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using BottleTrack.API.Iam.Domain.Repositories;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BottleTrack.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class OrganizationRepository : BaseRepository<Organization>, IOrganizationRepository
{
    public OrganizationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Organization?> FindByTaxIdAsync(TaxId taxId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Organization>()
            .FirstOrDefaultAsync(organization => organization.TaxId == taxId, cancellationToken);
    }
}
