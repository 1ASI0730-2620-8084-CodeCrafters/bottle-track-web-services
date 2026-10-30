using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using BottleTrack.API.Shared.Domain.Repositories;

namespace BottleTrack.API.Iam.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> ListByOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
