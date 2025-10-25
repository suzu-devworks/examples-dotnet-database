using System.Data;

#pragma warning disable IDE0130

namespace ContosoUniversity.Data;

public interface IDbConnectionProvider
{
    string ConnectionString { get; }
    int? CommandTimeout { get; }

    IDbConnection GetConnection();
    IDbConnection OpenConnection();
    ValueTask<IDbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default);
}
