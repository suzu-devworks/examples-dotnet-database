using System.Data;
using Examples.Dapper.Data;
using Npgsql;

namespace Examples.Dapper.PostgreSQL;

#pragma warning disable CS0618 // is obsolete

public class NpgsqlConnectionFactory(string connectionString)
    : IDbConnectionFactory
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
