using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Users.Commands.ConfirmEmail;
using Mailjet.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Resource;

namespace Auth.Api.Web.IntegrationTests.Users.Commands;

public class ConfirmEmailTests : UserEndpointsFixtures
{
    [Fact]
    public async Task ConfirmationEmail_ShouldConfirmEmail_WhenUserExists()
    {
        // Arrange
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        var emailConfirmationToken = await userManagerService.GenerateEmailConfirmationTokenAsync(UserId);

        emailConfirmationToken.Should().NotBeEmpty();

        var confirmEmailCommand = new ConfirmEmailCommand { Token = emailConfirmationToken! };

        // Act

        var emailConfirmationResult =
            await HttpClient.PostAsJsonAsync(ConfirmEmailUri, confirmEmailCommand);

        // Assert

        emailConfirmationResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ConfirmationEmail_ShouldNotConfirmEmail_WhenTokenIsInvalid()
    {
        // Arrange
        MailJetClientMock
            .Setup(x => x.PostAsync(It.IsAny<MailjetRequest>()))
            .ReturnsAsync(new MailjetResponse(true, 200, new JObject()));


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
