using BottleTrack.API.Fleet.Domain.Model.ValueObjects;
using Xunit;

namespace BottleTrack.API.Tests.Unit;

public class PlateNumberTests
{
    [Fact]
    public void Constructor_WithValidPlate_NormalizesToUppercase()
    {
        PlateNumber plate = new PlateNumber("abc-123");

        Assert.Equal("ABC-123", plate.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("ABC123")]
    [InlineData("ABCD-123")]
    [InlineData("AB!-123")]
    public void Constructor_WithInvalidPlate_ThrowsArgumentException(string value)
    {
        Assert.Throws<ArgumentException>(() => new PlateNumber(value));
    }
}
