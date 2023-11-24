using System.Net;
using System.Net.Http.Json;

namespace Auth.Api.Web.IntegrationTests.Users.Queries;

public class IsUserNameExistsTests : UserEndpointsFixtures
{
    [Fact]
    public async Task IsUserNameExists_ShouldReturnTrue_WhenUserNameIsAlreadyUsed()
    {
        // Arrange

        var registerUserCommand = GenerateRegisterUserCommand();

        await HttpClient.PostAsJsonAsync(RegisterUserUri, registerUserCommand);

        // Act

        var isUserNameExistsResponse =
            await HttpClient.GetAsync(IsUserNameExistsUri + "/" + registerUserCommand.UserName);
        var isUserNameExistsResult = await isUserNameExistsResponse.Content.ReadFromJsonAsync<bool>();

        // Assert
        isUserNameExistsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        isUserNameExistsResult.Should().BeTrue();
    }

    [Fact]
    public async Task IsUserNameExists_ShouldReturnFalse_WhenUserNameIsNotAlreadyUsed()
    {
        // Arrange
        var userName = new Faker().Internet.UserName();

        // Act

        var isUserNameExistsResponse = await HttpClient.GetAsync(IsUserNameExistsUri + "/" + userName);
        var isUserNameExistsResult = await isUserNameExistsResponse.Content.ReadFromJsonAsync<bool>();

        // Assert
        isUserNameExistsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        isUserNameExistsResult.Should().BeFalse();
    }
}
