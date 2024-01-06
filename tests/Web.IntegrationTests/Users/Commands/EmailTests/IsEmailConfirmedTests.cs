using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Mailjet.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Auth.Api.Web.IntegrationTests.Users.Commands.EmailTests;

public class IsEmailConfirmedTests : UserEndpointsFixtures
{
    public IsEmailConfirmedTests()
    {
        MailJetClientMock
            .Setup(x => x.PostAsync(It.IsAny<MailjetRequest>()))
            .ReturnsAsync(new MailjetResponse(true, 200, new JObject()));
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
        var isEmailConfirmedResponse = await HttpClient.GetAsync(IsEmailConfirmedUri);
        var isEmailConfirmedResult = await isEmailConfirmedResponse.Content.ReadFromJsonAsync<bool>();
        // Assert

        emailConfirmationResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        isEmailConfirmedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        isEmailConfirmedResult.Should().BeTrue();
    }
}
