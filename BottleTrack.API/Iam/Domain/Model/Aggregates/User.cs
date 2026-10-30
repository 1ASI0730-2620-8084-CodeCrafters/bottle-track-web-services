using BottleTrack.API.Iam.Domain.Model.ValueObjects;

namespace BottleTrack.API.Iam.Domain.Model.Aggregates;

public class User
{
    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public EmailAddress Email { get; private set; }

    public string PasswordHash { get; private set; }

    public UserRole Role { get; private set; }

    public UserStatus Status { get; private set; }

    private User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = null!;
        PasswordHash = string.Empty;
    }

    public static User Register(Guid organizationId, string firstName, string lastName, EmailAddress email,
        string passwordHash, UserRole role)
    {
        User user = new User();
        user.Id = Guid.NewGuid();
        user.OrganizationId = organizationId;
        user.FirstName = firstName.Trim();
        user.LastName = lastName.Trim();
        user.Email = email;
        user.PasswordHash = passwordHash;
        user.Role = role;
        user.Status = UserStatus.Active;
        return user;
    }

    public void AssignRole(UserRole role)
    {
        Role = role;
    }

    public void Deactivate()
    {
        Status = UserStatus.Inactive;
    }
}
