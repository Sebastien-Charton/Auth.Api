using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Users.Commands.EmailConfirmationToken;

namespace Auth.Api.Web.IntegrationTests.Users.Commands.EmailTests;

public class GetEmailConfirmationTokenTests : UserEndpointsFixtures
{
    [Fact]
    public async Task GetEmailConfirmationToken_ShouldReturnToken_WhenUserExists()
    {
        // Arrange
        var getEmailConfirmationTokenCommand = new GetEmailConfirmationTokenCommand();

        // Act

        var getEmailConfirmationTokenResponse =
            await HttpClient.PostAsJsonAsync(GetEmailConfirmationTokenUri, getEmailConfirmationTokenCommand);

        var getEmailConfirmationTokenResult = await getEmailConfirmationTokenResponse.Content
            .ReadFromJsonAsync<GetEmailConfirmationTokenResponse>();

        // Assert

        getEmailConfirmationTokenResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getEmailConfirmationTokenResult!.Token.Should().NotBeEmpty();
    }
}
