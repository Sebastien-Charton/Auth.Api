using Auth.Api.Application.Users.Commands.LoginUser;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Shared.Tests;

namespace Auth.Api.Application.FunctionalTests.Users.Commands.LoginUser;

public class LoginUserTests : BaseTestFixture
{
    [Fact]
    public async Task LoginUser_ShouldReturnToken_WhenUserExists()
    {
        // Arrange

        var registerUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();

        await SendAsync(registerUserCommand);

        var loginUserCommand = new LoginUserCommand
        {
            Email = registerUserCommand.Email, Password = registerUserCommand.Password
        };

        // Act

        var result = await SendAsync(loginUserCommand);

        // Assert

        result.Token.Should().NotBeNull();
    }

    [Fact]
    public async Task LoginUser_ShouldReturnUnauthorized_WhenUserDoesntExists()
    {
        // Arrange

        var loginUserCommand = new Faker<LoginUserCommand>()
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .Generate();

        // Act

        // Assert

        await FluentActions.Invoking(() =>
            SendAsync(loginUserCommand)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task LoginUser_ShouldReturnUnauthorized_WhenPasswordIsNotCorrect()
    {
        // Arrange

        var registerUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();

        await SendAsync(registerUserCommand);

        var loginUserCommand = new Faker<LoginUserCommand>()
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .RuleFor(x => x.Email, registerUserCommand.Email)
            .Generate();

        // Act

        // Assert

        await FluentActions.Invoking(() =>
            SendAsync(loginUserCommand)).Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public async Task LoginUser_ShouldLockAccount_WhenUserFailToLoginMultipleTimes(int maxRetry)
    {
        // Arrange

        var registerUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();

        await SendAsync(registerUserCommand);

        for (int i = 0; i < maxRetry; i++)
        {
            var loginUserCommandWithWrongPassword = new Faker<LoginUserCommand>()
                .RuleFor(x => x.Email, registerUserCommand.Email)
                .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
                .Generate();

            await FluentActions.Invoking(() =>
                SendAsync(loginUserCommandWithWrongPassword)
            ).Should().ThrowAsync<UnauthorizedAccessException>();
        }

        // Act

        var loginUserCommand = new LoginUserCommand
        {
            Email = registerUserCommand.Email, Password = registerUserCommand.Password
        };

        // Assert

        if (maxRetry < 5)
        {
            await FluentActions.Invoking(() =>
                SendAsync(loginUserCommand)
            ).Should().NotThrowAsync<UnauthorizedAccessException>();
        }
        else
        {
            await FluentActions.Invoking(() =>
                SendAsync(loginUserCommand)
            ).Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
