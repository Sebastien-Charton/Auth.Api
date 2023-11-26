using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Users.Commands.UpdatePassword;
using Auth.Api.Shared.Tests;

namespace Auth.Api.Web.IntegrationTests.Users.Commands;

public class UpdatePasswordTests : UserEndpointsFixtures
{
    [Fact]
    public async Task UpdatePassword_ShouldUpdatePassword_WhenNewPasswordIsValid()
    {
        // Arrange
        var updatePasswordCommand = new UpdatePasswordCommand
        {
            CurrentPassword = UserPassword, NewPassword = "Pass1!test"
        };

        // Act

        var updatePasswordResponse = await HttpClient.PostAsJsonAsync(UpdatePasswordUri, updatePasswordCommand);

        // Assert

        updatePasswordResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdatePassword_ShouldUpdatePassword_WhenCurrentPasswordIsInvalid()
    {
        // Arrange
        var updatePasswordCommand = new UpdatePasswordCommand
        {
            CurrentPassword = new Faker().Internet.GenerateCustomPassword(),
            NewPassword = new Faker().Internet.GenerateCustomPassword()
        };

        // Act

        var updatePasswordResponse = await HttpClient.PostAsJsonAsync(UpdatePasswordUri, updatePasswordCommand);

        // Assert

        updatePasswordResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
