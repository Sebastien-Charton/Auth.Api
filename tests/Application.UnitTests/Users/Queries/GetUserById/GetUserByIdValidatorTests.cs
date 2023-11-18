using Auth.Api.Application.Users.Queries.GetUserById;
using FluentAssertions;
using Xunit;

namespace Auth.Api.Application.UnitTests.Users.Queries.GetUserById;

public class GetUserByIdValidatorTests
{
    [Fact]
    public void GetUserByIdQueryValidator_ShouldBeValid_WhenDataAreCorrect()
    {
        // Arrange
        var getUserByIdQuery = new GetUserByIdQuery { Id = Guid.NewGuid() };

        var validator = new GetUserByIdQueryValidator();

        // Act

        var result = validator.Validate(getUserByIdQuery);

        // Assert

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void GetUserByIdQueryValidator_ShouldBeInvalid_WhenDataAreIncorrect()
    {
        // Arrange

        var getUserByIdQuery = new GetUserByIdQuery();

        var validator = new GetUserByIdQueryValidator();

        // Act

        var result = validator.Validate(getUserByIdQuery);

        // Assert

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
    }
}
