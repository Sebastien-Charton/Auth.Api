using Auth.Api.Application.Users.Queries.IsEmailExists;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Auth.Api.Application.UnitTests.Users.Queries.IsEmailExists;

public class IsEmailExistsQueryValidatorTests
{
    [Fact]
    public void IsEmailExistsQueryValidator_ShouldBeValid_WhenDataAreCorrect()
    {
        // Arrange
        var isEmailExistsQuery = new IsEmailExistsQuery { Email = new Faker().Internet.Email() };

        var validator = new IsEmailExistsQueryValidator();

        // Act

        var result = validator.Validate(isEmailExistsQuery);

        // Assert

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsEmailExistsQueryValidator_ShouldBeInvalid_WhenDataAreIncorrect()
    {
        // Arrange

        var isEmailExistsQuery = new IsEmailExistsQuery();

        var validator = new IsEmailExistsQueryValidator();

        // Act

        var result = validator.Validate(isEmailExistsQuery);

        // Assert

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
    }
}
