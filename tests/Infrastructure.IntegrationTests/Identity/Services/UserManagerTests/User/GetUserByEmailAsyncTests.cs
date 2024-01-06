using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests.User;

[Collection(nameof(UserManagerTests))]
public class GetUserByEmailAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange

        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.GetUserByEmailAsync(createUserResult.email);

        // Assert

        response.Should().NotBeNull();
        response!.UserName.Should().Be(createUserResult.userName);
        response.Email.Should().Be(createUserResult.email);
        response.Id.Should().Be(createUserResult.userId);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnNull_WhenUserDoesntExists()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.GetUserByEmailAsync("randomUserEmail@example.com");

        // Assert

        response.Should().BeNull();
    }
}
