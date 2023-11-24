﻿using System.Net.Http.Headers;
using Auth.Api.Application.Users.Commands.RegisterUser;
using Auth.Api.Infrastructure.Data;
using Auth.Api.Shared.Tests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Web.IntegrationTests;

public abstract class TestingFixture : IAsyncDisposable
{
    protected static readonly string BaseUri = "http://localhost/api/";
    private readonly ITestDatabase _database = null!;
    private readonly CustomWebApplicationFactory _factory = null!;
    private readonly IServiceScopeFactory _scopeFactory = null!;
    protected readonly Guid UserId;

    public TestingFixture()
    {
        _database = TestDatabaseFactory.CreateAsync().GetAwaiter().GetResult();

        _factory = new CustomWebApplicationFactory(_database.GetConnection(), ConfigureMocks());

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

        ApplicationDbContextInitialiser initialiser =
            _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        ServiceScope = _factory.Services.CreateScope();

        UserId = _factory.DefaultUserId;

        // Instantiate http client and mock Auth
        HttpClient = _factory.CreateClient();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        initialiser.SeedAsync().Wait();
    }

    public HttpClient HttpClient { get; private set; }

    protected IServiceScope ServiceScope { get; }

    public async ValueTask DisposeAsync()
    {
        await _database.DisposeAsync();
        await _factory.DisposeAsync();
    }

    protected virtual ServiceDescriptor[] ConfigureMocks()
    {
        return Array.Empty<ServiceDescriptor>();
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

    protected static RegisterUserCommand GenerateRegisterUserCommand()
    {
        return new Faker<RegisterUserCommand>()
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.UserName, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.GeneratePassword())
            .Generate();
    }
}
