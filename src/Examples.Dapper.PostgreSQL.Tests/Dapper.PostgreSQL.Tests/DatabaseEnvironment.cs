namespace Examples.Dapper.PostgreSQL.Tests;

public static class DatabaseEnvironment
{
    public static bool IsAvailable => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("POSTGRES_SERVICE"));

}
