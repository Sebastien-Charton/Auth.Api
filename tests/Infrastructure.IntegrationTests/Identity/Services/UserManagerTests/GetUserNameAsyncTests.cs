using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

[Collection(nameof(UserManagerTests))]
public class GetUserNameAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task GetUserNameAsync_ShouldGetUserName_WhenUserExists()
    {
        // Arrange

        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.GetUserNameAsync(createUserResult.userId);

        // Assert

        response.Should().NotBeNull();
        response.Should().Be(createUserResult.userName);
    }

    [Fact]
    public async Task GetUserNameAsync_ShouldReturnNull_WhenUserDoesntExists()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.GetUserNameAsync(Guid.NewGuid());

        // Assert

        response.Should().BeNull();
    }
}
