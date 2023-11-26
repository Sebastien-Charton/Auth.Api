using Auth.Api.Application.Users.Commands.ResetPassword;
using Auth.Api.Shared.Tests;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Auth.Api.Application.UnitTests.Users.Commands.ResetPassword;

public class ResetPasswordCommandValidatorTests
{
    [Fact]
    public void ResetPasswordCommandValidator_ShouldBeValid_WhenDataAreCorrect()
    {
        // Arrange

        var command = new ResetPasswordCommand
        {
            Token = new Faker().Lorem.Word(), NewPassword = new Faker().Internet.GenerateCustomPassword()
        };

        var validator = new ResetPasswordCommandValidator();

        // Act

        var result = validator.Validate(command);

        // Assert

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ResetPasswordCommandValidator_ShouldBeInvalid_WhenDataAreIncorrect()
    {
        // Arrange

        var updatePasswordCommand = new ResetPasswordCommand { Token = "", NewPassword = "" };

        var validator = new ResetPasswordCommandValidator();

        // Act

        var result = validator.Validate(updatePasswordCommand);

        // Assert

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(5);
    }
}
