using Auth.Api.Application.Common.Interfaces.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Infrastructure.Identity.Models;

// ReSharper disable once RedundantTypeDeclarationBody
public class ApplicationUser : IdentityUser<Guid>, IApplicationUser
{
}
