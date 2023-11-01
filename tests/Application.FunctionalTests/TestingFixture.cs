using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Application.FunctionalTests;

public class TestingFixture : IAsyncDisposable
{
    private readonly ITestDatabase _database = null!;
    private readonly CustomWebApplicationFactory _factory = null!;
    private readonly IServiceScopeFactory _scopeFactory = null!;

    public TestingFixture()
    {
        _database = TestDatabaseFactory.CreateAsync().GetAwaiter().GetResult();

        _factory = new CustomWebApplicationFactory(_database.GetConnection());

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

        ApplicationDbContextInitialiser initialiser =
            _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        initialiser.SeedAsync().Wait();
    }

    public async ValueTask DisposeAsync()
    {
        await _database.DisposeAsync();
        await _factory.DisposeAsync();
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        ISender mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public async Task SendAsync(IBaseRequest request)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        ISender mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        await mediator.Send(request);
    }

    public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    public async Task<string?> GenerateConfirmationEmail(Guid userId)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<IUserManagerService>();

        var token = await context.GenerateEmailConfirmation(userId);

        return token;
    }
}
