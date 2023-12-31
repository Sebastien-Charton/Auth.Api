﻿using Auth.Api.Application.Common.Interfaces.Identity.Models;
using Auth.Api.Application.Common.Models;

namespace Auth.Api.Application.Common.Interfaces.Identity.Services;

public interface IUserManagerService
{
    Task<string?> GetUserNameAsync(Guid userId);

    Task<bool> IsInRoleAsync(Guid userId, string role);

    Task<bool> AuthorizeAsync(Guid userId, string policyName);

    Task<(Result Result, Guid userId)> CreateUserAsync(string userName, string password, string email,
        string? phoneNumber);

    Task<Result> DeleteUserAsync(Guid userId);
    Task<List<string>> GetUserRolesAsync(IApplicationUser user);
    Task<Result> AddToRolesAsync(Guid userId, IEnumerable<string> roles);
    Task<IApplicationUser?> GetUserByEmailAsync(string email);
    Task<string?> GenerateEmailConfirmationTokenAsync(Guid userId);
    Task<Result> ConfirmEmailAsync(IApplicationUser userId, string token);
    Task<bool> IsEmailConfirmedAsync(Guid userId);
    Task<bool> IsUserNameExists(string userName);
    Task<bool> IsUserExists(Guid userId);
    Task<bool> IsEmailExists(string email);
    Task<IApplicationUser?> GetUserByIdAsync(Guid id);
    Task<Result> UpdateUserAsync(IApplicationUser user);
    Task<string> GenerateResetPasswordTokenAsync(IApplicationUser user);
    Task<Result> ResetPasswordAsync(IApplicationUser user, string token, string newPassword);
    Task<Result> ChangePasswordAsync(IApplicationUser user, string currentPassword, string newPassword);
    Task<Result> RemovePasswordAsync(IApplicationUser user);
}
