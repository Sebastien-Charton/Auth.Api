using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Auth.Api.Application.Users.Commands.IsEmailConfirmed;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Shared.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Web.IntegrationTests.Users.Commands;

public class IsEmailConfirmedTests : UserEndpointsFixtures
{
    [Fact]
    public async Task IsEmailConfirmed_ShouldReturnTrue_WhenEmailIsConfirmed()
    {
        // Arrange
        var registerUserCommand = GenerateRegisterUserCommand();

        var createUserResponse = await HttpClient.PostAsJsonAsync(RegisterUserUri, registerUserCommand);
        var userId = await createUserResponse.Content.ReadFromJsonAsync<Guid>();

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        var emailConfirmationToken = await userManagerService.GenerateEmailConfirmationToken(userId);

        emailConfirmationToken.Should().NotBeNull();
        emailConfirmationToken.Should().NotBeEmpty();

        var confirmEmailCommand = new ConfirmEmailCommand { Token = emailConfirmationToken!, UserId = userId };

        // Act

        var emailConfirmationResponse = await HttpClient.PostAsJsonAsync(ConfirmEmailUri, confirmEmailCommand);
        var emailConfirmationResult = await emailConfirmationResponse.Content.ReadFromJsonAsync<bool>();

        // Assert

        emailConfirmationResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        emailConfirmationResult.Should().BeTrue();
    }
}
