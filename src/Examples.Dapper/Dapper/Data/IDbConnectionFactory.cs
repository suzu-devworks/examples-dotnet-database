using System.Data;

namespace Examples.Dapper.Data;

[Obsolete("Unless there's a specific reason not to, please use System.Data.Common.DbDataSource.")]
public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
    ValueTask<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
