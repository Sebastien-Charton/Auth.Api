using Auth.Api.Application.Users.Queries.IsEmailConfirmed;
using FluentAssertions;
using Xunit;

namespace Auth.Api.Application.UnitTests.Users.Queries.IsEmailConfirmed;

public class IsEmailConfirmedValidatorTests
{
    [Fact]
    public void IsEmailConfirmedQueryValidator_ShouldBeValid_WhenDataAreCorrect()
    {
        // Arrange
        var isEmailConfirmedQuery = new IsEmailConfirmedQuery { UserId = Guid.NewGuid() };

        var validator = new IsEmailConfirmedQueryValidator();

        // Act

        var result = validator.Validate(isEmailConfirmedQuery);

        // Assert

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsEmailConfirmedValidator_ShouldBeInvalid_WhenDataAreIncorrect()
    {
        // Arrange

        var isEmailConfirmedQuery = new IsEmailConfirmedQuery();

        var validator = new IsEmailConfirmedQueryValidator();

        // Act

        var result = validator.Validate(isEmailConfirmedQuery);

        // Assert

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
    }
}
