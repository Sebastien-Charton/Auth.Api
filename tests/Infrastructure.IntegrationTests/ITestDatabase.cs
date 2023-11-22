using System.Data.Common;

namespace Auth.Api.Infrastructure.IntegrationTests;

public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetConnection();

    Task DisposeAsync();
}
