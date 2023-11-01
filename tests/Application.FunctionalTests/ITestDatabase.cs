using System.Data.Common;

namespace Auth.Api.Application.FunctionalTests;

public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetConnection();

    Task DisposeAsync();
}
