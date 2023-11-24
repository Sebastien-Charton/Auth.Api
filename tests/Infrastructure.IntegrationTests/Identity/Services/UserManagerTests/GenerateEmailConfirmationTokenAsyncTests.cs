using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

public class GenerateEmailConfirmationTokenAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task GenerateEmailConfirmationTokenAsync_ShouldReturnToken_WhenUserExists()
    {
        // Arrange
        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.GenerateEmailConfirmationTokenAsync(createUserResult.userId);

        // Assert

        response.Should().NotBeEmpty();
    }
}
