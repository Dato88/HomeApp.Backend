using Domain.ValueObjects;

namespace ApplicationTests.UnitTests.Finance;

public class IbanTests
{
    [Theory]
    [InlineData("DE89370400440532013000")]
    [InlineData("de89 3704 0044 0532 0130 00")]
    [InlineData("DE44 5001 0517 5407 3249 31")]
    [InlineData("GB29 NWBK 6016 1331 9268 19")]
    public void IsValid_ShouldAcceptValidIbans(string iban) =>
        Iban.IsValid(iban).Should().BeTrue();

    [Theory]
    [InlineData("DE89370400440532013001")]
    [InlineData("DE00 1234")]
    [InlineData("NotAnIban")]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_ShouldRejectInvalidIbans(string iban) =>
        Iban.IsValid(iban).Should().BeFalse();

    [Fact]
    public void Normalize_ShouldRemoveWhitespaceAndUppercase() =>
        Iban.Normalize("de44 5001 0517 5407 3249 31").Should().Be("DE44500105175407324931");
}
