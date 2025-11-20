using System.Data;

namespace Examples.Dapper.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
    ValueTask<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
