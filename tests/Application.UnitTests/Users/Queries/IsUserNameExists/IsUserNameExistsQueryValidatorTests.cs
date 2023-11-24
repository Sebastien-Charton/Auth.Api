using Auth.Api.Application.Users.Queries.IsUserNameExists;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Auth.Api.Application.UnitTests.Users.Queries.IsUserNameExists;

public class IsUserNameExistsQueryValidatorTests
{
    [Fact]
    public void IsUserNameExistsQueryValidator_ShouldBeValid_WhenDataAreCorrect()
    {
        // Arrange
        var isUserNameExistsQuery = new IsUserNameExistsQuery { UserName = new Faker().Internet.UserName() };

        var validator = new IsUserNameExistsQueryValidator();

        // Act

        var result = validator.Validate(isUserNameExistsQuery);

        // Assert

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsUserNameExistsQueryValidator_ShouldBeInvalid_WhenDataAreIncorrect()
    {
        // Arrange

        var isUserNameExistsQuery = new IsUserNameExistsQuery();

        var validator = new IsUserNameExistsQueryValidator();

        // Act

        var result = validator.Validate(isUserNameExistsQuery);

        // Assert

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
    }
}
