using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Shared.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Web.IntegrationTests.Users;

public class ConfirmEmailTests : UserEndpointsFixtures
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

        var createUserResult =
            await HttpClient.PostAsJsonAsync(RegisterUserUri, registerUserCommand);

        var userId = await createUserResult.Content.ReadFromJsonAsync<Guid>();

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        var emailConfirmationToken = await userManagerService.GenerateEmailConfirmationToken(userId);

        emailConfirmationToken.Should().NotBeEmpty();

        var confirmEmailCommand = new ConfirmEmailCommand { Token = emailConfirmationToken!, UserId = userId };

        // Act

        var emailConfirmationResult =
            await HttpClient.PostAsJsonAsync(ConfirmEmailUri, confirmEmailCommand);

        var isConfirmed = await emailConfirmationResult.Content.ReadFromJsonAsync<bool>();

        // Assert

        isConfirmed.Should().BeTrue();
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

        var emailConfirmationResult =
            await HttpClient.PostAsJsonAsync(ConfirmEmailUri, confirmEmailCommand);

        // Assert

        emailConfirmationResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
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

        var emailConfirmationResult =
            await HttpClient.PostAsJsonAsync(ConfirmEmailUri, confirmEmailCommand);

        // Assert

        emailConfirmationResult.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await emailConfirmationResult.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails!.Status.Should().Be(400);
    }
}
