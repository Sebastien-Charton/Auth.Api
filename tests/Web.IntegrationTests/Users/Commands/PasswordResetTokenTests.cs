using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Users.Commands.EmailConfirmationToken;
using Auth.Api.Application.Users.Commands.PasswordResetToken;

namespace Auth.Api.Web.IntegrationTests.Users.Commands;

public class PasswordResetTokenTests : UserEndpointsFixtures
{
    [Fact]
    public async Task GetEmailConfirmationToken_ShouldReturnToken_WhenUserExists()
    {
        // Arrange
        var getEmailConfirmationTokenCommand = new GetEmailConfirmationTokenCommand();

        // Act

        var getPasswordResetTokenResponse =
            await HttpClient.PostAsJsonAsync(GetPasswordResetToken, new PasswordResetTokenCommand());

        var getPasswordResetTokenResult = await getPasswordResetTokenResponse.Content
            .ReadFromJsonAsync<GetEmailConfirmationTokenResponse>();

        // Assert

        getPasswordResetTokenResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getPasswordResetTokenResult!.Token.Should().NotBeEmpty();
    }
}
