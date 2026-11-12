using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using BottleTrack.API.Iam.Domain.Repositories;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BottleTrack.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> FindByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>()
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<User>> ListByOrganizationAsync(Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>()
            .Where(user => user.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }
}
