using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

[Collection(nameof(UserManagerTests))]
public class IsUserNameExistsAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task IsUserNameExistsAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange

        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.IsUserNameExists(createUserResult.userName);

        // Assert

        response.Should().BeTrue();
    }

    [Fact]
    public async Task IsUserNameExistsAsync_ShouldReturnFalse_WhenUserDoesntExists()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.IsUserNameExists("randomUserName");

        // Assert

        response.Should().BeFalse();
    }
}
