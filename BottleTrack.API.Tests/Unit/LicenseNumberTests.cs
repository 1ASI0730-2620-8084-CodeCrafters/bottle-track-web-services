using BottleTrack.API.Fleet.Domain.Model.ValueObjects;
using Xunit;

namespace BottleTrack.API.Tests.Unit;

public class LicenseNumberTests
{
    [Fact]
    public void Constructor_WithValidLicense_NormalizesToUppercase()
    {
        LicenseNumber license = new LicenseNumber("q12345678");

        Assert.Equal("Q12345678", license.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("12345678")]
    [InlineData("Q1234567")]
    [InlineData("QA2345678")]
    public void Constructor_WithInvalidLicense_ThrowsArgumentException(string value)
    {
        Assert.Throws<ArgumentException>(() => new LicenseNumber(value));
    }
}
