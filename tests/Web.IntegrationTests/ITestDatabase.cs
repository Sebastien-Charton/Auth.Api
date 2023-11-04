using System.Data.Common;

namespace Auth.Api.Web.IntegrationTests;

public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetConnection();

    Task DisposeAsync();
}
