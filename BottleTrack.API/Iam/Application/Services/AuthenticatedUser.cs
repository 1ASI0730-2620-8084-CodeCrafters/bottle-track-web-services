using BottleTrack.API.Iam.Domain.Model.Aggregates;

namespace BottleTrack.API.Iam.Application.Services;

public class AuthenticatedUser
{
    public User User { get; }

    public string Token { get; }

    public AuthenticatedUser(User user, string token)
    {
        User = user;
        Token = token;
    }
}
