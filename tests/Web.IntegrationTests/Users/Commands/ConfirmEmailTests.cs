using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Resource;

namespace Auth.Api.Web.IntegrationTests.Users.Commands;

public class ConfirmEmailTests : UserEndpointsFixtures
{
    [Fact]
    public async Task ConfirmationEmail_ShouldConfirmEmail_WhenUserExists()
    {
        // Arrange
        var registerUserCommand = GenerateRegisterUserCommand();

        var createUserResult =
            await HttpClient.PostAsJsonAsync(RegisterUserUri, registerUserCommand);

        var userId = await createUserResult.Content.ReadFromJsonAsync<Guid>();

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        var emailConfirmationToken = await userManagerService.GenerateEmailConfirmationTokenAsync(userId);

        emailConfirmationToken.Should().NotBeEmpty();

        var confirmEmailCommand = new ConfirmEmailCommand { Token = emailConfirmationToken! };

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
        var confirmEmailCommand = new ConfirmEmailCommand { Token = "token" };

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
        var registerUserCommand = GenerateRegisterUserCommand();

        var userId = await SendAsync(registerUserCommand);

        var confirmEmailCommand = new ConfirmEmailCommand { Token = "token" };

        // Act

        var emailConfirmationResult =
            await HttpClient.PostAsJsonAsync(ConfirmEmailUri, confirmEmailCommand);

        // Assert

        emailConfirmationResult.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await emailConfirmationResult.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails!.Should().NotBeNull();
        problemDetails!.Detail.Should().Be(UserErrorMessages.InvalidToken);
    }
}
