using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.Queries;

namespace BottleTrack.API.Iam.Application.Services;

public interface IUserQueryService
{
    Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> Handle(GetUsersByOrganizationIdQuery query, CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> Handle(GetUsersByRoleQuery query, CancellationToken cancellationToken = default);
}
