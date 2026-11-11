using BottleTrack.API.Iam.Infrastructure.Hashing;
using Xunit;

namespace BottleTrack.API.Tests.Unit;

public class Pbkdf2PasswordHasherTests
{
    private readonly Pbkdf2PasswordHasher hasher = new Pbkdf2PasswordHasher();

    [Fact]
    public void Verify_WithCorrectPassword_ReturnsTrue()
    {
        string hash = hasher.Hash("Password123");

        Assert.True(hasher.Verify("Password123", hash));
    }

    [Fact]
    public void Verify_WithWrongPassword_ReturnsFalse()
    {
        string hash = hasher.Hash("Password123");

        Assert.False(hasher.Verify("WrongPassword", hash));
    }
}
