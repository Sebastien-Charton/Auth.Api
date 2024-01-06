using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests.Email;

[Collection(nameof(UserManagerTests))]
public class IsEmailConfirmedAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task IsEmailConfirmedAsync_ShouldReturnTrue_WhenEmailIsConfirmed()
    {
        // Arrange
        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();
        var user = await userManagerService.GetUserByIdAsync(createUserResult.userId);
        var token = await userManagerService.GenerateEmailConfirmationTokenAsync(createUserResult.userId);
        await userManagerService.ConfirmEmailAsync(user!, token!);
        // Act

        var isEmailConfirmedResponse = await userManagerService.IsEmailConfirmedAsync(createUserResult.userId);

        // Assert

        isEmailConfirmedResponse.Should().BeTrue();
    }

    [Fact]
    public async Task IsEmailConfirmedAsync_ShouldReturnFalse_WhenEmailIsNotConfirmed()
    {
        // Arrange
        var createUserResult = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var isEmailConfirmedResponse = await userManagerService.IsEmailConfirmedAsync(createUserResult.userId);

        // Assert

        isEmailConfirmedResponse.Should().BeFalse();
    }
}
