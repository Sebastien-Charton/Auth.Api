using Auth.Api.Domain.Exceptions;
using Auth.Api.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Auth.Api.Domain.UnitTests.ValueObjects;

public class ColourTests
{
    [Fact]
    public void ShouldReturnCorrectColourCode()
    {
        string code = "#FFFFFF";

        Colour colour = Colour.From(code);

        colour.Code.Should().Be(code);
    }

    [Fact]
    public void ToStringReturnsCode()
    {
        Colour colour = Colour.White;

        colour.ToString().Should().Be(colour.Code);
    }

    [Fact]
    public void ShouldPerformImplicitConversionToColourCodeString()
    {
        string code = Colour.White;

        code.Should().Be("#FFFFFF");
    }

    [Fact]
    public void ShouldPerformExplicitConversionGivenSupportedColourCode()
    {
        Colour colour = (Colour)"#FFFFFF";

        colour.Should().Be(Colour.White);
    }

    [Fact]
    public void ShouldThrowUnsupportedColourExceptionGivenNotSupportedColourCode()
    {
        FluentActions.Invoking(() => Colour.From("##FF33CC"))
            .Should().Throw<UnsupportedColourException>();
    }
}
