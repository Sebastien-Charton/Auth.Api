using Auth.Api.Application.Users.Commands.UpdatePassword;
using Auth.Api.Shared.Tests;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Auth.Api.Application.UnitTests.Users.Commands.UpdatePassword;

public class UpdatePasswordCommandValidatorTests
{
    [Fact]
    public void UpdatePasswordCommandValidator_ShouldBeValid_WhenDataAreCorrect()
    {
        // Arrange

        var command = new UpdatePasswordCommand
        {
            CurrentPassword = new Faker().Internet.GenerateCustomPassword(),
            NewPassword = new Faker().Internet.GenerateCustomPassword()
        };

        var validator = new UpdatePasswordCommandValidator();

        // Act

        var result = validator.Validate(command);

        // Assert

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void UpdatePasswordCommandValidator_ShouldBeInvalid_WhenDataAreIncorrect()
    {
        // Arrange

        var updatePasswordCommand = new UpdatePasswordCommand { CurrentPassword = "", NewPassword = "" };

        var validator = new UpdatePasswordCommandValidator();

        // Act

        var result = validator.Validate(updatePasswordCommand);

        // Assert

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(8);
    }
}
