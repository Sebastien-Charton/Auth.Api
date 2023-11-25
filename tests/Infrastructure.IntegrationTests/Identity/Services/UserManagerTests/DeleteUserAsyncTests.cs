using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

[Collection(nameof(UserManagerTests))]
public class DeleteUserAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task DeleterUserAsync_ShouldReturnSuccess_WhenUserIsDeleted()
    {
        // Arrange
        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.DeleteUserAsync(createUserResult.userId);

        // Assert

        response.Succeeded.Should().BeTrue();
    }
}
