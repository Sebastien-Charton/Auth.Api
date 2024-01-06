using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Users.Commands.SendPasswordResetToken;
using Mailjet.Client;
using Newtonsoft.Json.Linq;

namespace Auth.Api.Web.IntegrationTests.Users.Commands.PasswordTests;

public class SendPasswordResetTokenTests : UserEndpointsFixtures
{
    [Fact]
    public async Task SendPasswordResetToken_ShouldSendToken_WhenUserExists()
    {
        // Arrange
        MailJetClientMock
            .Setup(x => x.PostAsync(It.IsAny<MailjetRequest>()))
            .ReturnsAsync(new MailjetResponse(true, 200, new JObject()));

        var sendPasswordResetTokenCommand = new SendPasswordResetTokenCommand();

        // Act

        var sendPasswordResetTokenResponse =
            await HttpClient.PostAsJsonAsync(SendPasswordResetTokenUri, sendPasswordResetTokenCommand);

        // Assert

        sendPasswordResetTokenResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        MailJetClientMock
            .Verify(x => x.PostAsync(It.IsAny<MailjetRequest>()), Times.Once);
    }
}
