using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Users.Commands.ConfirmEmail;
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

        var emailConfirmationToken = await userManagerService.GenerateEmailConfirmationTokenAsync(userId);

        emailConfirmationToken.Should().NotBeNull();
        emailConfirmationToken.Should().NotBeEmpty();

        var confirmEmailCommand = new ConfirmEmailCommand { Token = emailConfirmationToken! };

        // Act

        var emailConfirmationResponse = await HttpClient.PostAsJsonAsync(ConfirmEmailUri, confirmEmailCommand);
        var emailConfirmationResult = await emailConfirmationResponse.Content.ReadFromJsonAsync<bool>();
        var isEmailConfirmedResponse = await HttpClient.GetAsync(IsEmailConfirmedUri + "/" + userId);
        var isEmailConfirmedResult = await isEmailConfirmedResponse.Content.ReadFromJsonAsync<bool>();
        // Assert

        emailConfirmationResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        emailConfirmationResult.Should().BeTrue();

        isEmailConfirmedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        isEmailConfirmedResult.Should().BeTrue();
    }
}
