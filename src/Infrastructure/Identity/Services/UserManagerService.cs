using System.Security.Claims;
using Auth.Api.Application.Common.Interfaces.Identity.Models;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Common.Models;
using Auth.Api.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Infrastructure.Identity.Services;

public class UserManagerService : IUserManagerService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserManagerService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    public async Task<string?> GetUserNameAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return user?.UserName;
    }

    public async Task<bool> IsUserNameExists(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        return user is not null;
    }

    public async Task<bool> IsUserExists(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return user is not null;
    }

    public async Task<bool> IsEmailExists(string userName)
    {
        var user = await _userManager.FindByEmailAsync(userName);

        return user is not null;
    }

    public async Task<IApplicationUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IApplicationUser?> GetUserByIdAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString());
    }

    public async Task<(Result Result, Guid userId)> CreateUserAsync(string userName, string password, string email,
        string? phoneNumber)
    {
        ApplicationUser user = new() { UserName = userName, Email = email, PhoneNumber = phoneNumber };

        IdentityResult result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(Guid userId, string role)
    {
        ApplicationUser? user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user is not null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
    {
        ApplicationUser? user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is null)
        {
            return false;
        }

        ClaimsPrincipal principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        AuthorizationResult result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(Guid userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<List<string>> GetUserRolesAsync(IApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync((ApplicationUser)user);
        return roles.ToList();
    }

    public async Task<Result> AddToRolesAsync(Guid userId, IEnumerable<string> roles)
    {
        var user = await GetUserAsync(userId);
        var result = await _userManager.AddToRolesAsync((ApplicationUser)user!, roles);
        return result.ToApplicationResult();
    }

    public async Task<string?> GenerateEmailConfirmationToken(Guid userId)
    {
        var user = await GetUserAsync(userId);

        if (user is null)
        {
            return null;
        }

        return await _userManager.GenerateEmailConfirmationTokenAsync((ApplicationUser)user);
    }

    public async Task<Result> ConfirmEmailAsync(IApplicationUser user, string token)
    {
        var result = await _userManager.ConfirmEmailAsync((ApplicationUser)user, token);

        return result.ToApplicationResult();
    }

    public async Task<bool> IsEmailConfirmed(Guid userId)
    {
        var user = await GetUserAsync(userId);

        if (user is null)
        {
            return false;
        }

        var result = await _userManager.IsEmailConfirmedAsync((ApplicationUser)user);

        return result;
    }

    public async Task<IApplicationUser?> GetUserAsync(Guid userId)
    {
        return await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        IdentityResult result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }
}
