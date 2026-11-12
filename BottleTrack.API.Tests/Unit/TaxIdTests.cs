using BottleTrack.API.Iam.Domain.Model.ValueObjects;
using Xunit;

namespace BottleTrack.API.Tests.Unit;

public class TaxIdTests
{
    [Fact]
    public void Constructor_WithElevenDigits_KeepsValue()
    {
        TaxId taxId = new TaxId("20123456789");

        Assert.Equal("20123456789", taxId.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("201234567890")]
    [InlineData("2012345678A")]
    public void Constructor_WithInvalidTaxId_ThrowsArgumentException(string value)
    {
        Assert.Throws<ArgumentException>(() => new TaxId(value));
    }
}
