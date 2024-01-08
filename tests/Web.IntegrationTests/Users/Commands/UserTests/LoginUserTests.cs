using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Users.Commands.LoginUser;
using Auth.Api.Shared.Tests;

namespace Auth.Api.Web.IntegrationTests.Users.Commands.UserTests;

public class LoginUserTests : UserEndpointsFixtures
{
    [Fact]
    public async Task LoginUser_ShouldReturnToken_WhenUserExists()
    {
        // Arrange
        var registerUserCommand = GenerateRegisterUserCommand();

        await HttpClient.PostAsJsonAsync(RegisterUserUri, registerUserCommand);

        var loginUserCommand = new LoginUserCommand
        {
            Email = registerUserCommand.Email, Password = registerUserCommand.Password
        };

        // Act

        var loginUserResponse = await HttpClient.PostAsJsonAsync(LoginUserUri, loginUserCommand);
        var loginUserResult = await loginUserResponse.Content.ReadFromJsonAsync<LoginUserResponse>();

        // Assert

        loginUserResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        loginUserResult!.Token.Should().NotBeEmpty();
        loginUserResult.RefreshToken.Should().NotBeEmpty();
    }

    [Fact]
    public async Task LoginUser_ShouldReturnUnauthorized_WhenUserDoesntExists()
    {
        // Arrange

        var loginUserCommand = new Faker<LoginUserCommand>()
            .RuleFor(x => x.Password, f => f.Internet.GenerateCustomPassword())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .Generate();

        // Act

        var loginUserResponse = await HttpClient.PostAsJsonAsync(LoginUserUri, loginUserCommand);

        // Assert

        loginUserResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LoginUser_ShouldReturnUnauthorized_WhenPasswordIsNotCorrect()
    {
        // Arrange
        var registerUserCommand = GenerateRegisterUserCommand();

        await HttpClient.PostAsJsonAsync(RegisterUserUri, registerUserCommand);

        var loginUserCommand = new Faker<LoginUserCommand>()
            .RuleFor(x => x.Password, f => f.Internet.GenerateCustomPassword())
            .RuleFor(x => x.Email, registerUserCommand.Email)
            .Generate();

        // Act

        var loginUserResponse = await HttpClient.PostAsJsonAsync(LoginUserUri, loginUserCommand);

        // Assert

        loginUserResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData(5)]
    public async Task LoginUser_ShouldLockAccount_WhenUserFailToLoginMultipleTimes(int maxRetry)
    {
        // Arrange

        var registerUserCommand = GenerateRegisterUserCommand();

        await HttpClient.PostAsJsonAsync(RegisterUserUri, registerUserCommand);

        var loginUserCommandWithWrongPassword = new Faker<LoginUserCommand>()
            .RuleFor(x => x.Email, registerUserCommand.Email)
            .RuleFor(x => x.Password, f => f.Internet.GenerateCustomPassword())
            .Generate();

        for (int i = 0; i < maxRetry; i++)
        {
            await HttpClient.PostAsJsonAsync(LoginUserUri, loginUserCommandWithWrongPassword);
        }

        // Act

        var loginUserCommand = new LoginUserCommand
        {
            Email = registerUserCommand.Email, Password = registerUserCommand.Password
        };

        // Assert

        var loginUserResponse = await HttpClient.PostAsJsonAsync(LoginUserUri, loginUserCommand);

        loginUserResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
