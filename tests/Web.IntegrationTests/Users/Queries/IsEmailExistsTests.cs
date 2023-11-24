using System.Net;
using System.Net.Http.Json;

namespace Auth.Api.Web.IntegrationTests.Users.Queries;

public class IsEmailExistsTests : UserEndpointsFixtures
{
    [Fact]
    public async Task IsEmailExists_ShouldReturnTrue_WhenEmailIsAlreadyUsed()
    {
        // Arrange

        var registerUserCommand = GenerateRegisterUserCommand();

        await HttpClient.PostAsJsonAsync(RegisterUserUri, registerUserCommand);

        // Act

        var isEmailExistsResponse = await HttpClient.GetAsync(IsEmailExistsUri + "/" + registerUserCommand.Email);
        var isEmailExistsResult = await isEmailExistsResponse.Content.ReadFromJsonAsync<bool>();

        // Assert
        isEmailExistsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        isEmailExistsResult.Should().BeTrue();
    }

    [Fact]
    public async Task IsEmailExists_ShouldReturnFalse_WhenEmailIsNotAlreadyUsed()
    {
        // Arrange
        var email = new Faker().Internet.Email();

        // Act

        var isEmailExistsResponse = await HttpClient.GetAsync(IsEmailExistsUri + "/" + email);
        var isEmailExistsResult = await isEmailExistsResponse.Content.ReadFromJsonAsync<bool>();

        // Assert
        isEmailExistsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        isEmailExistsResult.Should().BeFalse();
    }
}
