using Auth.Api.Application.Users.Commands.LoginUser;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Auth.Api.Application.UnitTests.Users.Commands.LoginUser;

public class LoginUserCommandValidatorTests
{
    [Fact]
    public void LoginUserCommandValidator_ShouldBeValid_WhenDataAreCorrect()
    {
        // Arrange

        var loginUserCommandValidator = new Faker<LoginUserCommand>()
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Password, f => f.Internet.Password())
            .Generate();

        var validator = new LoginUserCommandValidator();

        // Act

        var result = validator.Validate(loginUserCommandValidator);

        // Assert

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void LoginUserCommandValidator_ShouldBeInvalid_WhenDataAreIncorrect()
    {
        // Arrange

        var confirmEmailCommand = new LoginUserCommand();

        var validator = new LoginUserCommandValidator();

        // Act

        var result = validator.Validate(confirmEmailCommand);

        // Assert

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(2);
    }
}
