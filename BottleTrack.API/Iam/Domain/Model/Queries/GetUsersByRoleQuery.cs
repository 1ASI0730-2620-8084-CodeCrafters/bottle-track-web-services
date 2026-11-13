using BottleTrack.API.Iam.Domain.Model.ValueObjects;

namespace BottleTrack.API.Iam.Domain.Model.Queries;

public class GetUsersByRoleQuery
{
    public Guid OrganizationId { get; }

    public UserRole Role { get; }

    public GetUsersByRoleQuery(Guid organizationId, UserRole role)
    {
        OrganizationId = organizationId;
        Role = role;
    }
}
