using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

[Collection(nameof(UserManagerTests))]
public class IsEmailExistsAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task IsEmailExistsAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange

        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.IsEmailExists(createUserResult.email);

        // Assert

        response.Should().BeTrue();
    }

    [Fact]
    public async Task IsEmailExistsAsync_ShouldReturnFalse_WhenUserDoesntExists()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.IsEmailExists("randomUserEmail@example.com");

        // Assert

        response.Should().BeFalse();
    }
}
