using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

public class UpdateUserAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateUser_WhenUserPropertiesChanged()
    {
        // Arrange
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        var createdUser = await CreateUser();

        var createdUserResponse = await userManagerService.GetUserByIdAsync(createdUser.userId);

        var newUserName = new Faker().Internet.UserName();
        createdUserResponse!.UserName = newUserName;

        // Act

        var updateUserResponse = await userManagerService.UpdateUserAsync(createdUserResponse);
        var getUserByIdUpdatedResponse = await userManagerService.GetUserByIdAsync(createdUser.userId);

        // Assert

        updateUserResponse.Succeeded.Should().BeTrue();
        getUserByIdUpdatedResponse!.UserName.Should().Be(newUserName);
    }
}
