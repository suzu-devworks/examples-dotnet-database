using Microsoft.Data.SqlClient;

namespace Examples.Dapper.SqlServer.Tests.Dialect;

public class ConnectionStringTests(ITestOutputHelper output)
{
    [Fact]
    public void When_TimeoutExplicitlySpecifiedInConnectionString_Then_TimeoutPropertyMatchesSpecifiedValue()
    {
        var builder = new SqlConnectionStringBuilder("Data Source=MSSQL1;Initial Catalog=AdventureWorks;");

        using (var connection = new SqlConnection(builder.ConnectionString))
        using (var command = connection.CreateCommand())
        {
            output.WriteLine($"Default ConnectionString: {connection.ConnectionString}");
            Assert.Equal(15, connection.ConnectionTimeout);
            Assert.Equal(30, command.CommandTimeout);
        }

        var updated = new SqlConnectionStringBuilder(builder.ConnectionString)
        {
            ConnectTimeout = 123,
            CommandTimeout = 345
        };

        using (var connection = new SqlConnection(updated.ConnectionString))
        using (var command = connection.CreateCommand())
        {
            output.WriteLine($"Updated ConnectionString: {connection.ConnectionString}");
            Assert.Equal(123, connection.ConnectionTimeout);
            Assert.Equal(345, command.CommandTimeout);
        }

    }
}
