using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

public class IsUserExistsAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task IsUserExistsAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange

        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.IsUserExists(createUserResult.userId);

        // Assert

        response.Should().BeTrue();
    }

    [Fact]
    public async Task IsUserExistsAsync_ShouldReturnFalse_WhenUserDoesntExists()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.IsUserExists(Guid.NewGuid());

        // Assert

        response.Should().BeFalse();
    }
}
