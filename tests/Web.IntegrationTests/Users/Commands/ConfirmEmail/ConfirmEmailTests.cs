using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Shared.Tests;
using Microsoft.Extensions.DependencyInjection;
using Resource;

namespace Auth.Api.Web.IntegrationTests.Users.Commands.ConfirmEmail;

public class ConfirmEmailTests : TestingFixture
{
    [Fact]
    public async Task ConfirmationEmail_ShouldConfirmEmail_WhenUserExists()
    {
        // Arrange
        RegisterUserCommand? registerUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();

        var userId = await SendAsync(registerUserCommand);

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        var emailConfirmationToken = await userManagerService.GenerateEmailConfirmationToken(userId);

        emailConfirmationToken.Should().NotBeNull();
        emailConfirmationToken.Should().NotBeEmpty();

        var confirmEmailCommand = new ConfirmEmailCommand { Token = emailConfirmationToken!, UserId = userId };

        // Act

        var emailConfirmationResult = await SendAsync(confirmEmailCommand);

        // Assert

        emailConfirmationResult.Should().BeTrue();
    }

    [Fact]
    public async Task ConfirmationEmail_ShouldThrowError_WhenUserDoesntExists()
    {
        // Arrange
        RegisterUserCommand? registerUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();

        var confirmEmailCommand = new ConfirmEmailCommand { Token = "token", UserId = Guid.NewGuid() };

        // Act

        // Assert

        await FluentActions
            .Invoking(() => SendAsync(confirmEmailCommand))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ConfirmationEmail_ShouldNotConfirmEmail_WhenTokenIsInvalid()
    {
        // Arrange
        RegisterUserCommand? registerUserCommand = new Faker<RegisterUserCommand>()
            .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();

        var userId = await SendAsync(registerUserCommand);

        var confirmEmailCommand = new ConfirmEmailCommand { Token = "token", UserId = userId };

        // Act

        // Assert

        await FluentActions.Invoking(() =>
                SendAsync(confirmEmailCommand))
            .Should().ThrowAsync<Exception>(UserErrorMessages.InvalidToken);
    }
}
