using BottleTrack.API.Iam.Domain.Model.ValueObjects;

namespace BottleTrack.API.Iam.Domain.Model.Commands;

public class RegisterUserCommand
{
    public Guid OrganizationId { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Email { get; }

    public string Password { get; }

    public UserRole Role { get; }

    public RegisterUserCommand(Guid organizationId, string firstName, string lastName, string email, string password,
        UserRole role)
    {
        OrganizationId = organizationId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Role = role;
    }
}
