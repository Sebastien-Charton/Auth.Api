using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Shared.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

[Collection(nameof(UserManagerTests))]
public class ResetPasswordAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task ResetPasswordAsync_ShouldResetToken_WhenTokenIsValid()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();
        var createUserResult = await CreateUser();

        var user = await userManagerService.GetUserByIdAsync(createUserResult.userId);

        var resetPasswordToken = await userManagerService.GenerateResetPasswordTokenAsync(user!);

        var newPassword = new Faker().Internet.GenerateCustomPassword();

        // Act

        var resetPasswordResult = await userManagerService.ResetPasswordAsync(user!, resetPasswordToken, newPassword);

        // Assert

        resetPasswordResult.Succeeded.Should().BeTrue();
    }
}
