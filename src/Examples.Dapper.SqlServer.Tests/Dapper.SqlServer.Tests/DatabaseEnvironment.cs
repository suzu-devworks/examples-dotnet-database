namespace Examples.Dapper.SqlServer.Tests;

public static class DatabaseEnvironment
{
    public static bool IsAvailable => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MSSQL_SERVICE"));

}
