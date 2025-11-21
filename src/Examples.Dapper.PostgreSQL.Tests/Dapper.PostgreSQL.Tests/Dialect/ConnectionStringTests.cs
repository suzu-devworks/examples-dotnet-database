using Npgsql;

namespace Examples.Dapper.PostgreSQL.Tests.Dialect;

public class ConnectionStringTests(ITestOutputHelper output)
{
    [Fact]
    public void When_TimeoutExplicitlySpecifiedInConnectionString_Then_TimeoutPropertyMatchesSpecifiedValue()
    {
        var builder = new NpgsqlConnectionStringBuilder("Host=localhost");

        using (var connection = new NpgsqlConnection(builder.ConnectionString))
        using (var command = connection.CreateCommand())
        {
            output.WriteLine($"Default ConnectionString: {connection.ConnectionString}");
            Assert.Equal(15, connection.ConnectionTimeout);
            Assert.Equal(30, command.CommandTimeout);
        }

        var updated = new NpgsqlConnectionStringBuilder(builder.ConnectionString)
        {
            Timeout = 123,
            CommandTimeout = 345
        };

        using (var connection = new NpgsqlConnection(updated.ConnectionString))
        using (var command = connection.CreateCommand())
        {
            output.WriteLine($"Updated ConnectionString: {connection.ConnectionString}");
            Assert.Equal(123, connection.ConnectionTimeout);
            Assert.Equal(345, command.CommandTimeout);
        }

    }
}
