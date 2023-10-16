using Auth.Api.Application.Common.Interfaces.Identity.Models;
using Auth.Api.Application.Common.Models;

namespace Auth.Api.Application.Common.Interfaces.Identity.Services;

public interface IUserManagerService
{
    Task<string?> GetUserNameAsync(Guid userId);

    Task<bool> IsInRoleAsync(Guid userId, string role);

    Task<bool> AuthorizeAsync(Guid userId, string policyName);

    Task<(Result Result, Guid userId)> CreateUserAsync(string userName, string password, string email,
        string phoneNumber);

    Task<Result> DeleteUserAsync(Guid userId);
    Task<string?> GetUserByUserNameAsync(string userName);
    Task<List<string>> GetUserRolesAsync(IApplicationUser user);
    Task<Result> AddToRolesAsync(Guid userId, IEnumerable<string> roles);
    Task<IApplicationUser?> GetUserByEmailAsync(string email);
}
