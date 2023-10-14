using System.Data.Common;
using Auth.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Respawn;
using Respawn.Graph;

namespace Auth.Api.Application.FunctionalTests;

public class PostgreSqlTestDatabase : ITestDatabase
{
    private readonly string _connectionString = null!;
    private DbConnection _connection = null!;
    private Respawner _respawner = null!;

    public PostgreSqlTestDatabase()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString);

        _connectionString = connectionString;
    }

    public async Task InitialiseAsync()
    {
        _connection = new NpgsqlConnection(_connectionString);

        await _connection.OpenAsync();

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_connectionString)
            .Options;

        ApplicationDbContext context = new(options);

        await context.Database.MigrateAsync();

        _respawner = await Respawner.CreateAsync(_connection,
            new RespawnerOptions
            {
                TablesToIgnore = new Table[] { "__EFMigrationsHistory" }, DbAdapter = DbAdapter.Postgres
            });
    }

    public DbConnection GetConnection()
    {
        return _connection;
    }

    public async Task ResetAsync()
    {
        await _respawner.ResetAsync(_connectionString);
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}
