using Domain.ValueObjects;

namespace ApplicationTests.UnitTests.Finance;

public class PartnerNameTests
{
    [Theory]
    [InlineData("REWE Markt GmbH", "REWE MARKT GMBH")]
    [InlineData("  Vermieter   Müller  ", "VERMIETER MÜLLER")]
    [InlineData("Bäckerei\tSchäfer\nGmbH", "BÄCKEREI SCHÄFER GMBH")]
    [InlineData("rewe", "REWE")]
    public void Normalize_ShouldTrimCollapseAndUppercase(string input, string expected) =>
        PartnerName.Normalize(input).Should().Be(expected);

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t\n")]
    public void Normalize_ShouldReturnNullForBlankValues(string? input) =>
        PartnerName.Normalize(input).Should().BeNull();
}
