using Auth.Api.Domain.Entities;

namespace Auth.Api.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<RefreshToken> RefreshTokens { get; }
}
