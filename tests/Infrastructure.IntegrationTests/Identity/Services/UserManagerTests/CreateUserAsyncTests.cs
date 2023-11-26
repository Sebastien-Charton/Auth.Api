using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Shared.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

[Collection(nameof(UserManagerTests))]
public class CreateUserAsyncTests : UserManagerTestsFixtures
{
    [Fact]
    public async Task CreateUserAsync_ShouldCreateUser_WhenUserIsValid()
    {
        // Arrange

        var email = new Faker().Person.Email;
        var userName = new Faker().Internet.UserName();
        var password = new Faker().Internet.GenerateCustomPassword();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.CreateUserAsync(userName, password, email, null);

        // Assert

        response.Result.Succeeded.Should().BeTrue();
        response.userId.Should().NotBeEmpty();
    }
}
