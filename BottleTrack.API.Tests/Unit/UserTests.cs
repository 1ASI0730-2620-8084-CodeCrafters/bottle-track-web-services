using BottleTrack.API.Iam.Domain.Model.Aggregates;
using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using Xunit;

namespace BottleTrack.API.Tests.Unit;

public class UserTests
{
    [Fact]
    public void Register_SetsIdentityAndActiveStatus()
    {
        EmailAddress email = new EmailAddress("angel@codecrafters.pe");

        User user = User.Register(Guid.NewGuid(), " Angel ", " Crispin ", email, "hash", UserRole.Administrator);

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("Angel", user.FirstName);
        Assert.Equal("Crispin", user.LastName);
        Assert.Equal(UserRole.Administrator, user.Role);
        Assert.Equal(UserStatus.Active, user.Status);
    }

    [Fact]
    public void Deactivate_ChangesStatusToInactive()
    {
        EmailAddress email = new EmailAddress("angel@codecrafters.pe");
        User user = User.Register(Guid.NewGuid(), "Angel", "Crispin", email, "hash", UserRole.Administrator);

        user.Deactivate();

        Assert.Equal(UserStatus.Inactive, user.Status);
    }
}
