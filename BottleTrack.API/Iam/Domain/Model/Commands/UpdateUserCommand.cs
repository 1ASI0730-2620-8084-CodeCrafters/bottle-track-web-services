namespace BottleTrack.API.Iam.Domain.Model.Commands;

public class UpdateUserCommand
{
    public Guid UserId { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Email { get; }

    public UpdateUserCommand(Guid userId, string firstName, string lastName, string email)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}
