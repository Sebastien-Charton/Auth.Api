namespace Auth.Api.Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
        TestcontainersTestDatabase database = new();

        await database.InitialiseAsync();

        return database;
    }
}
