using Auth.Api.Infrastructure.Services.HtmlGeneration;
using FluentAssertions;
using Xunit;

namespace Auth.Api.Infrastructure.UnitTests.Services.HtmlGeneration;

public class DotLiquidHtmlGeneratorTests
{
    [Fact]
    public void GenerateConfirmationEmail_ShouldGenerateHtml_WhenDataAreValid()
    {
        // Arrange

        var userName = "unit-test";
        var token = Guid.NewGuid().ToString();

        var htmlGenerator = new DotLiquidHtmlGenerator();
        // Act

        var result = htmlGenerator.GenerateConfirmationEmail(userName, token);

        // Assert
        result.Should().Contain(userName);
        result.Should().Contain(token);
    }
}
