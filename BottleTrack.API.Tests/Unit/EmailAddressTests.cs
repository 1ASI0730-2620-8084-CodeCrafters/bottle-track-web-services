using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using Xunit;

namespace BottleTrack.API.Tests.Unit;

public class EmailAddressTests
{
    [Fact]
    public void Constructor_WithValidEmail_NormalizesToLowercase()
    {
        EmailAddress email = new EmailAddress("  Angel@CodeCrafters.PE ");

        Assert.Equal("angel@codecrafters.pe", email.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("without-at-sign")]
    [InlineData("@codecrafters.pe")]
    [InlineData("angel@")]
    public void Constructor_WithInvalidEmail_ThrowsArgumentException(string value)
    {
        Assert.Throws<ArgumentException>(() => new EmailAddress(value));
    }
}
