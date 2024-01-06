using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Domain.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests.User;

[Collection(nameof(UserManagerTests))]
public class IsInRoleAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task IsInRoleAsync_ShouldReturnTrue_WhenUserIsInRole()
    {
        // Arrange
        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        await userManagerService.AddToRolesAsync(createUserResult.userId, new[] { Roles.User });
        // Act

        var response = await userManagerService.IsInRoleAsync(createUserResult.userId, Roles.User);

        // Assert

        response.Should().BeTrue();
    }

    [Fact]
    public async Task IsInRoleAsync_ShouldReturnFalse_WhenUserIsNotInRole()
    {
        // Arrange
        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        await userManagerService.AddToRolesAsync(createUserResult.userId, new[] { Roles.User });
        // Act

        var response = await userManagerService.IsInRoleAsync(createUserResult.userId, Roles.Administrator);

        // Assert

        response.Should().BeFalse();
    }
}
