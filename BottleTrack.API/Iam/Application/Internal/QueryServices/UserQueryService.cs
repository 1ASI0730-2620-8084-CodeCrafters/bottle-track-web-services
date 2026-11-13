using BottleTrack.API.Iam.Application.Services;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Queries;
using BottleTrack.API.Iam.Domain.Repositories;

namespace BottleTrack.API.Iam.Application.Internal.QueryServices;

public class UserQueryService : IUserQueryService
{
    private readonly IUserRepository userRepository;

    public UserQueryService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await userRepository.FindByIdAsync(query.UserId, cancellationToken);
    }

    public async Task<IEnumerable<User>> Handle(GetUsersByOrganizationIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await userRepository.ListByOrganizationAsync(query.OrganizationId, cancellationToken);
    }

    public async Task<IEnumerable<User>> Handle(GetUsersByRoleQuery query, CancellationToken cancellationToken = default)
    {
        IEnumerable<User> users = await userRepository.ListByOrganizationAsync(query.OrganizationId, cancellationToken);

        List<User> usersWithRole = new List<User>();
        foreach (User user in users)
        {
            if (user.Role == query.Role)
                usersWithRole.Add(user);
        }

        return usersWithRole;
    }
}
