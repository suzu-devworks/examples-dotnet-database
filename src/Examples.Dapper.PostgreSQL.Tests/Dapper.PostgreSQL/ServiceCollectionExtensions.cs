using Examples.Dapper.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbConnectionFactory(this IServiceCollection services, Action<DbConnectionFactoryOptionsBuilder>? optionsAction)
    {
        global::Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var builder = new DbConnectionFactoryOptionsBuilder();

        optionsAction?.Invoke(builder);

        var connectionString = builder.ConnectionString
            ?? throw new InvalidOperationException("ConnectionString is not set.");

        services.AddSingleton<IDbConnectionFactory>(provider =>
            new NpgsqlConnectionFactory(connectionString));

        return services;
    }

    public sealed class DbConnectionFactoryOptionsBuilder
    {
        public string? ConnectionString { get; private set; }

        public DbConnectionFactoryOptionsBuilder UseNpgsql(string connectionString)
        {
            ConnectionString = connectionString;
            return this;
        }
    }

}
