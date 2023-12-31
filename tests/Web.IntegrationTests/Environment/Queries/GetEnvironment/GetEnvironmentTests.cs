﻿using System.Net.Http.Json;
using Auth.Api.Application.Environment.Queries;

namespace Auth.Api.Web.IntegrationTests.Environment.Queries.GetEnvironment;

public class GetEnvironmentTests : TestingFixture
{
    [Fact]
    public async Task GetEnvironment_ShouldRetrieveEnvironment_WhenEnvIsSetup()
    {
        var result =
            await HttpClient.GetFromJsonAsync<GetEnvironmentDto>(new Uri("http://localhost/api/Environment"),
                CancellationToken.None);

        result.Should().NotBeNull();
        result!.Environment.Should().Be("Test");
    }
}
