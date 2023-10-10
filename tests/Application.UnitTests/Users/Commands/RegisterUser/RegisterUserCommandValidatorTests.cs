using Auth.Api.Application.Users.Commands.RegisterUser;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Auth.Api.Application.UnitTests.Users.Commands.RegisterUser;

public class RegisterUserCommandValidatorTests
{
    [Fact]
    public void RegisterUserCommandValidator_ShouldBeValid_WhenDataAreCorrect()
    {
        // Arrange

        var registerUserCommand = new RegisterUserCommand()
        {
            Email = "example@gmail.com", Password = "Password1*,", UserName = "TEST", PhoneNumber = "+35905458484"
        };

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
