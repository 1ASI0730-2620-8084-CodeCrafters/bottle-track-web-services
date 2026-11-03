using BottleTrack.API.Iam.Domain.Model.ValueObjects;

namespace BottleTrack.API.Iam.Interfaces.Rest.Resources;

public class UserResource
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public UserStatus Status { get; set; }
}
