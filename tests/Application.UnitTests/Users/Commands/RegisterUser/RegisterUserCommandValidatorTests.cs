﻿using Auth.Api.Application.Users.Commands.RegisterUser;
using Bogus;
using FluentAssertions;
using Shared.Tests;
using Xunit;

namespace Auth.Api.Application.UnitTests.Users.Commands.RegisterUser;

public class RegisterUserCommandValidatorTests
{
    [Fact]
    public void RegisterUserCommandValidator_ShouldBeValid_WhenDataAreCorrect()
    {
        // Arrange

        RegisterUserCommand? registerUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();

        var validator = new RegisterUserCommandValidator();

        // Act

        var result = validator.Validate(registerUserCommand);

        // Assert

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void RegisterUserCommandValidator_ShouldBeInvalid_WhenDataAreIncorrect()
    {
        // Arrange

        var registerUserCommand = new RegisterUserCommand()
        {
            Email = "", Password = "", UserName = "", PhoneNumber = ""
        };

        var validator = new RegisterUserCommandValidator();

        // Act

        var result = validator.Validate(registerUserCommand);

        // Assert

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(11);
    }
}
