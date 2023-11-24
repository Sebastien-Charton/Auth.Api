using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Common.Models;
using Auth.Api.Shared.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infrastructure.IntegrationTests.Identity.Services.UserManagerTests;

public class UserManagerTestsFixtures : TestingFixture
{
    protected async Task<(string email, string userName, string password, Result result, Guid userId)> CreateUser()
    {
        var email = new Faker().Person.Email;
        var userName = new Faker().Internet.UserName();
        var password = new Faker().Internet.GeneratePassword();
        var userManagerService = ServiceScope.ServiceProvider.GetRequiredService<IUserManagerService>();

        var response = await userManagerService.CreateUserAsync(userName, password, email, null);

        return (email, userName, password, response.Result, response.userId);
    }
}
