using Auth.Api.Application.Users.Commands.RegisterUserAdmin;
using Auth.Api.Domain.Constants;
using Auth.Api.Shared.Tests;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Auth.Api.Application.UnitTests.Users.Commands.RegisterUserAdminAdmin;

public class RegisterUserAdminCommandValidatorTests
{
    [Fact]
    public void RegisterUserAdminCommandValidator_ShouldBeValid_WhenDataAreCorrect()
    {
        // Arrange

        RegisterUserAdminCommand? registerUserCommand = new Faker<RegisterUserAdminCommand>()
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GenerateCustomPassword())
            .RuleFor(x => x.Roles, Roles.GetRoles)
            .Generate();

        var validator = new RegisterUserAdminCommandValidator();

        // Act

        var result = validator.Validate(registerUserCommand);

        // Assert

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void RegisterUserAdminCommandValidator_ShouldBeInvalid_WhenDataAreIncorrect()
    {
        // Arrange

        var registerUserAdminCommand =
            new RegisterUserAdminCommand { Email = "", Password = "", UserName = "", Roles = Array.Empty<string>() };

        var validator = new RegisterUserAdminCommandValidator();

        // Act

        var result = validator.Validate(registerUserAdminCommand);

        // Assert

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(11);
    }
}
