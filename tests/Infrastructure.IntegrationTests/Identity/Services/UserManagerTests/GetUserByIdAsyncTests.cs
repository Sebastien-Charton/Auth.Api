using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

[Collection(nameof(UserManagerTests))]
public class GetUserByIdAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange

        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.GetUserByIdAsync(createUserResult.userId);

        // Assert

        response.Should().NotBeNull();
        response!.UserName.Should().Be(createUserResult.userName);
        response!.Email.Should().Be(createUserResult.email);
        response!.Id.Should().Be(createUserResult.userId);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesntExists()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.GetUserByIdAsync(Guid.NewGuid());

        // Assert

        response.Should().BeNull();
    }
}
