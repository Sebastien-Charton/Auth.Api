using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Shared.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests.Password;

[Collection(nameof(UserManagerTests))]
public class ChangePasswordAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task ChangePasswordAsync_ShouldUpdatePassword_WhenPasswordIsValid()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();
        var createUserResult = await CreateUser();
        var user = await userManagerService.GetUserByIdAsync(createUserResult.userId);

        var newPassword = new Faker().Internet.GenerateCustomPassword();

        // Act

        var changePasswordResult =
            await userManagerService.ChangePasswordAsync(user!, createUserResult.password, newPassword);

        // Assert

        changePasswordResult.Succeeded.Should().BeTrue();
    }
}
