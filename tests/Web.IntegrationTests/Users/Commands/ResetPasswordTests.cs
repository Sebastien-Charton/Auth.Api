using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Users.Commands.EmailConfirmationToken;
using Auth.Api.Application.Users.Commands.PasswordResetToken;
using Auth.Api.Application.Users.Commands.ResetPassword;
using Auth.Api.Shared.Tests;

namespace Auth.Api.Web.IntegrationTests.Users.Commands;

public class ResetPasswordTests : UserEndpointsFixtures
{
    [Fact]
    public async Task ResetPassword_ShouldResetPassword_WhenTokenIsValid()
    {
        // Arrange

        var getPasswordResetTokenResponse =
            await HttpClient.PostAsJsonAsync(GetPasswordResetTokenUri, new PasswordResetTokenCommand());

        var getPasswordResetTokenResult = await getPasswordResetTokenResponse.Content
            .ReadFromJsonAsync<GetEmailConfirmationTokenResponse>();

        var resetPasswordCommand = new ResetPasswordCommand
        {
            NewPassword = new Faker().Internet.GenerateCustomPassword(), Token = getPasswordResetTokenResult!.Token
        };

        // Act

        var resetPasswordResponse = await HttpClient.PutAsJsonAsync(ResetPasswordUri, resetPasswordCommand);

        // Assert

        resetPasswordResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
