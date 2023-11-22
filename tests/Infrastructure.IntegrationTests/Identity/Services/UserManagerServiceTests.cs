using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Common.Models;
using Auth.Api.Shared.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services;

public class UserManagerServiceTests : TestingFixture
{
    [Fact]
    public async Task GetUserNameAsync_ShouldGetUserName_WhenUserExists()
    {
        // Arrange

        var createUserResponse = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.GetUserNameAsync(createUserResponse.userId);

        // Assert

        response.Should().NotBeNull();
        response.Should().Be(createUserResponse.userName);
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

    [Fact]
    public async Task IsUserNameExistsAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange

        var createUserResponse = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.IsUserNameExists(createUserResponse.userName);

        // Assert

        response.Should().BeTrue();
    }

    [Fact]
    public async Task IsUserNameExistsAsync_ShouldReturnFalse_WhenUserDoesntExists()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.IsUserNameExists("randomUserName");

        // Assert

        response.Should().BeFalse();
    }

    [Fact]
    public async Task IsUserExistsAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange

        var createUserResponse = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.IsUserExists(createUserResponse.userId);

        // Assert

        response.Should().BeTrue();
    }

    [Fact]
    public async Task IsUserExistsAsync_ShouldReturnFalse_WhenUserDoesntExists()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.IsUserExists(Guid.NewGuid());

        // Assert

        response.Should().BeFalse();
    }

    [Fact]
    public async Task IsEmailExistsAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange

        var createUserResponse = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.IsEmailExists(createUserResponse.email);

        // Assert

        response.Should().BeTrue();
    }

    [Fact]
    public async Task IsEmailExistsAsync_ShouldReturnFalse_WhenUserDoesntExists()
    {
        // Arrange

        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.IsEmailExists("randomUserEmail@example.com");

        // Assert

        response.Should().BeFalse();
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange

        var createUserResponse = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.GetUserByEmailAsync(createUserResponse.email);

        // Assert

        response.Should().NotBeNull();
        response!.UserName.Should().Be(createUserResponse.userName);
        response!.Email.Should().Be(createUserResponse.email);
        response!.Id.Should().Be(createUserResponse.userId);
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

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange

        var createUserResponse = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.GetUserByIdAsync(createUserResponse.userId);

        // Assert

        response.Should().NotBeNull();
        response!.UserName.Should().Be(createUserResponse.userName);
        response!.Email.Should().Be(createUserResponse.email);
        response!.Id.Should().Be(createUserResponse.userId);
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

    [Fact]
    public async Task CreateUserAsync_ShouldCreateUser_WhenUserIsValid()
    {
        // Arrange

        var email = new Faker().Person.Email;
        var userName = new Faker().Internet.UserName();
        var password = new Faker().Internet.GeneratePassword();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        // Act

        var response = await userManagerService.CreateUserAsync(userName, password, email, null);

        // Assert

        response.Result.Succeeded.Should().BeTrue();
        response.userId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task IsInRoleAsync_ShouldReturnTrue_WhenUserIsInRole()
    {
        // Arrange
        const string role = "Test";

        var createUserResponse = await CreateUser();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        await userManagerService.AddToRolesAsync(createUserResponse.userId, new[] { role });
        // Act

        var response = await userManagerService.IsInRoleAsync(createUserResponse.userId, role);

        // Assert

        response.Should().BeTrue();
    }

    private async Task<(string email, string userName, string password, Result result, Guid userId)> CreateUser()
    {
        var email = new Faker().Person.Email;
        var userName = new Faker().Internet.UserName();
        var password = new Faker().Internet.GeneratePassword();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        var response = await userManagerService.CreateUserAsync(userName, password, email, null);

        return (email, userName, password, response.Result, response.userId);
    }
}
