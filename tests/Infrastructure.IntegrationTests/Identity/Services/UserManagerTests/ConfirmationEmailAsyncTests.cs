using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

[Collection(nameof(UserManagerTests))]
public class ConfirmationEmailAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task ConfirmationEmailAsync_ShouldConfirmEmail_WhenUserExists()
    {
        // Arrange
        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();
        var user = await userManagerService.GetUserByIdAsync(createUserResult.userId);
        var token = await userManagerService.GenerateEmailConfirmationTokenAsync(createUserResult.userId);

        // Act

        var response = await userManagerService.ConfirmEmailAsync(user!, token!);

        // Assert

        response.Succeeded.Should().BeTrue();
    }
}
