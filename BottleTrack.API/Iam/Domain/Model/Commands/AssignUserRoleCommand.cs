using BottleTrack.API.Iam.Domain.Model.ValueObjects;

namespace BottleTrack.API.Iam.Domain.Model.Commands;

public class AssignUserRoleCommand
{
    public Guid UserId { get; }

    public UserRole Role { get; }

    public AssignUserRoleCommand(Guid userId, UserRole role)
    {
        UserId = userId;
        Role = role;
    }
}
