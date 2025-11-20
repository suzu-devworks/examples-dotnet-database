using Examples.Dapper.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Examples.Dapper.PostgreSQL;

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
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);

        // .NET 6.0+

        // It feels redundant.
        services.AddKeyedSingleton<NpgsqlDataSource>(
            serviceKey,
            (provider, key) =>
                new NpgsqlDataSourceBuilder(connectionStringBuilder.ConnectionString)
                    .UseLoggerFactory(provider.GetRequiredService<ILoggerFactory>())
                    .Build());

        services.AddKeyedSingleton<System.Data.Common.DbDataSource>(
            serviceKey,
            (service, key) =>
                service.GetRequiredKeyedService<NpgsqlDataSource>(key));

        return services;
    }

    public static IServiceCollection AddDbConnectionFactory(this IServiceCollection services, Action<DbConnectionFactoryOptionsBuilder>? optionsAction)
    {
        global::Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        var builder = new DbConnectionFactoryOptionsBuilder();

        optionsAction?.Invoke(builder);

        var connectionString = builder.ConnectionString
            ?? throw new InvalidOperationException("ConnectionString is not set.");

        // Previous versions

#pragma warning disable CS0618 // is obsolete
        services.AddSingleton<IDbConnectionFactory>(provider =>
            new NpgsqlConnectionFactory(connectionString));
#pragma warning restore CS0618 // is obsolete

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
