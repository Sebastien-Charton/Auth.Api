using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Web.IntegrationTests.Users.Commands;

public class IsEmailConfirmedTests : UserEndpointsFixtures
{
    private readonly RegisterUserCommand _registerUserCommand = GenerateRegisterUserCommand();

    public IsEmailConfirmedTests()
    {
        var createUserResult = HttpClient
            .PostAsJsonAsync(RegisterUserUri, _registerUserCommand)
            .GetAwaiter()
            .GetResult();

        var userId = createUserResult.Content
            .ReadFromJsonAsync<Guid>()
            .GetAwaiter()
            .GetResult();
    }

    [Fact]
    public async Task IsEmailConfirmed_ShouldReturnTrue_WhenEmailIsConfirmed()
    {
        // Arrange
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        var emailConfirmationToken = await userManagerService.GenerateEmailConfirmationTokenAsync(UserId);

        emailConfirmationToken.Should().NotBeNull();
        emailConfirmationToken.Should().NotBeEmpty();

        var confirmEmailCommand = new ConfirmEmailCommand { Token = emailConfirmationToken! };

        // Act

        var emailConfirmationResponse = await HttpClient.PostAsJsonAsync(ConfirmEmailUri, confirmEmailCommand);
        var emailConfirmationResult = await emailConfirmationResponse.Content.ReadFromJsonAsync<bool>();
        var isEmailConfirmedResponse = await HttpClient.GetAsync(IsEmailConfirmedUri);
        var isEmailConfirmedResult = await isEmailConfirmedResponse.Content.ReadFromJsonAsync<bool>();
        // Assert

        emailConfirmationResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        emailConfirmationResult.Should().BeTrue();

        isEmailConfirmedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        isEmailConfirmedResult.Should().BeTrue();
    }
}
