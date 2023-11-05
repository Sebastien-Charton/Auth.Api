using System.Net.Http.Json;
using Auth.Api.Application.Environment.Queries;

namespace Auth.Api.Web.IntegrationTests.Environment.Queries.GetEnvironment;

public class GetEnvironementTests : TestingFixture
{
    [Fact]
    public async Task GetEnvironmentTests()
    {
        var result =
            await HttpClient.GetFromJsonAsync<GetEnvironmentDto>(new Uri("http://localhost/api/EnvironmentEndpoints"),
                CancellationToken.None);

        result.Should().NotBeNull();
        result!.Environment.Should().Be("Test");
    }
}
