using System.Data;
using ContosoUniversity.Data;
using Npgsql;

namespace Examples.ContosoUniversity.PostgreSQL.Data;

public class NpgsqlConnectionProvider(
    string connectionString
    ) : IDbConnectionProvider
{
    public string ConnectionString { get; } = new NpgsqlConnectionStringBuilder(connectionString).ConnectionString;
    public int? CommandTimeout => 100;

    static NpgsqlConnectionProvider()
    {
        global::Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public IDbConnection GetConnection()
    {
        return new NpgsqlConnection(ConnectionString);
    }

    public IDbConnection OpenConnection()
    {
        var connection = GetConnection();
        connection.Open();
        return connection;
    }
}
