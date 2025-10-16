using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Examples.EntityFrameworkCore.PostgreSQL.Tests;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder<TContext> UseNpgsqlDefault<TContext>(this DbContextOptionsBuilder<TContext> options,
        string? connectionString = null,
        Action<NpgsqlDbContextOptionsBuilder>? optionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseNpgsqlDefault((DbContextOptionsBuilder)options, connectionString, optionsAction);

    public static DbContextOptionsBuilder UseNpgsqlDefault(this DbContextOptionsBuilder options,
        string? connectionString = null,
        Action<NpgsqlDbContextOptionsBuilder>? optionsAction = null)
    {
        connectionString ??= "Host=postgres;Database=examples;Username=postgres;Password=pg@Password";
        options.UseNpgsql(connectionString, optionsAction);

        return options;
    }

}
