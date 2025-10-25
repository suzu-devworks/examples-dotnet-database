using System.Data;
using System.Data.Common;
using ContosoUniversity.Data;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Examples.ContosoUniversity.PostgreSQL.Data;

public class NpgsqlConnectionProvider : IDbConnectionProvider, IDisposable
{
    private readonly DbDataSource _dataSource;

    static NpgsqlConnectionProvider()
    {
        global::Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public NpgsqlConnectionProvider(string connectionString, ILoggerFactory loggerFactory)
    {
        var builder = new NpgsqlDataSourceBuilder(connectionString)
              .UseLoggerFactory(loggerFactory);

        _dataSource = builder.Build();
    }
    public void Dispose()
    {
        _dataSource.Dispose();
        GC.SuppressFinalize(this);
    }

    public string ConnectionString => _dataSource.ConnectionString;
    public int? CommandTimeout => 100;

    public IDbConnection GetConnection()
    {
        return _dataSource.CreateConnection();
    }

    public IDbConnection OpenConnection()
    {
        return _dataSource.OpenConnection();
    }

    public async ValueTask<IDbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        return connection;
    }

}
