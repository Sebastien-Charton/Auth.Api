using Auth.Api.Application.Users.Commands.DeleteUser;
using FluentAssertions;
using Xunit;

namespace Auth.Api.Application.UnitTests.Users.Commands.DeleteUser;

public class DeleteUserCommandValidatorTests
{
    [Fact]
    public void DeleteUserCommandValidator_ShouldBeValid_WhenDataAreCorrect()
    {
        // Arrange

        var command = new DeleteUserCommand(Guid.NewGuid());

        var validator = new DeleteUserCommandValidator();

        // Act

        var result = validator.Validate(command);

        // Assert

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void DeleteUserCommandValidator_ShouldBeInvalid_WhenDataAreIncorrect()
    {
        // Arrange

        var deleteUserCommand = new DeleteUserCommand(Guid.Empty);

        var validator = new DeleteUserCommandValidator();

        // Act

        var result = validator.Validate(deleteUserCommand);

        // Assert

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
    }
}
