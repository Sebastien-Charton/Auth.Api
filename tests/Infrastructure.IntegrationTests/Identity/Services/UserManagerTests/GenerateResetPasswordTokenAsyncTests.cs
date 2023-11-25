using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

[Collection(nameof(UserManagerTests))]
public class GenerateResetPasswordTokenAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task GenerateResetPasswordTokenAsync_ShouldGetResetPasswordToken_WhenUserIsValid()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();
        var createUserResult = await CreateUser();

        var user = await userManagerService.GetUserByIdAsync(createUserResult.userId);

        // Act

        var response = await userManagerService.GenerateResetPasswordTokenAsync(user!);

        // Assert

        response.Should().NotBeEmpty();
    }
}
