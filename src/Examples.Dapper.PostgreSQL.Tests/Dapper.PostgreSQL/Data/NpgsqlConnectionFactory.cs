using System.Data;
using Npgsql;

namespace Examples.Dapper.PostgreSQL.Data;

public class NpgsqlConnectionFactory(string connectionString)
#pragma warning disable CS0618
    : IDbConnectionFactory
#pragma warning restore CS0618
{
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }

    public async ValueTask<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }

}
