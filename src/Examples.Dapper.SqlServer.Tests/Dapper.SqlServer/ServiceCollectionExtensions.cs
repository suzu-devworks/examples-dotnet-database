using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.SqlServer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbDataSource(this IServiceCollection services, Action<DbConnectionFactoryOptionsBuilder>? optionsAction)
        => services.AddKeyedDbDataSource(null, optionsAction);

    public static IServiceCollection AddKeyedDbDataSource(this IServiceCollection services, object? serviceKey, Action<DbConnectionFactoryOptionsBuilder>? optionsAction)
    {
        global::Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var optionsBuilder = new DbConnectionFactoryOptionsBuilder();

        optionsAction?.Invoke(optionsBuilder);

        var connectionString = optionsBuilder.ConnectionString
                ?? throw new InvalidOperationException("ConnectionString is not set.");
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

        services.AddKeyedSingleton<System.Data.Common.DbDataSource>(
            serviceKey,
            (service, key) => SqlClientFactory.Instance.CreateDataSource(connectionStringBuilder.ConnectionString));

        return services;
    }

    public sealed class DbConnectionFactoryOptionsBuilder
    {
        public string? ConnectionString { get; private set; }

        public DbConnectionFactoryOptionsBuilder UseSqlServer(string connectionString)
        {
            ConnectionString = connectionString;
            return this;
        }
    }

}
