using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Users.Commands.SendConfirmationEmailToken;
using Mailjet.Client;
using Newtonsoft.Json.Linq;

namespace Auth.Api.Web.IntegrationTests.Users.Commands;

public class SendEmailConfirmationTokenTests : UserEndpointsFixtures
{
    [Fact]
    public async Task SendEmailConfirmationToken_ShouldSendToken_WhenUserExists()
    {
        // Arrange
        MailJetClientMock
            .Setup(x => x.PostAsync(It.IsAny<MailjetRequest>()))
            .ReturnsAsync(new MailjetResponse(true, 200, new JObject()));

        var sendConfirmationEmailTokenCommand = new SendConfirmationEmailTokenCommand();

        // Act

        var sendConfirmationEmailTokenResponse =
            await HttpClient.PostAsJsonAsync(SendEmailConfirmationTokenUri, sendConfirmationEmailTokenCommand);

        // Assert

        sendConfirmationEmailTokenResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        MailJetClientMock
            .Verify(x => x.PostAsync(It.IsAny<MailjetRequest>()), Times.Once);
    }
}
