using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests.Password;

[Collection(nameof(UserManagerTests))]
public class RemovePasswordAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task RemovePasswordAsync_ShouldRemovePassword_WhenUserExists()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();
        var createUserResult = await CreateUser();
        var user = await userManagerService.GetUserByIdAsync(createUserResult.userId);

        // Act

        var removePasswordResult =
            await userManagerService.RemovePasswordAsync(user!);

        // Assert

        removePasswordResult.Succeeded.Should().BeTrue();
    }
}
