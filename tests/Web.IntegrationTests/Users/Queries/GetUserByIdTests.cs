using System.Net;
using System.Net.Http.Json;
using Auth.Api.Application.Users.Queries.GetUserById;

namespace Auth.Api.Web.IntegrationTests.Users.Queries;

public class GetUserByIdTests : UserEndpointsFixtures
{
    [Fact]
    public async Task GetUserById_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        // Act
        var getUserResponse =
            await HttpClient.GetAsync(new Uri(GetUserByIdUri + "/" + UserId),
                CancellationToken.None);
        var getUserResult = await getUserResponse.Content.ReadFromJsonAsync<GetUserByIdResponse>();

        // Assert
        getUserResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getUserResult.Should().NotBeNull();
        getUserResult!.Id.Should().Be(UserId);
    }

    [Fact]
    public async Task GetUserById_ShouldNotReturnUser_WhenUserNotExists()
    {
        // Arrange
        // Act
        var getUserResponse =
            await HttpClient.GetAsync(new Uri(GetUserByIdUri + "/" + Guid.NewGuid()),
                CancellationToken.None);
        var getUserResult = await getUserResponse.Content.ReadFromJsonAsync<GetUserByIdResponse>();

        // Assert
        getUserResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        getUserResult.Should().NotBeNull();
    }
}
