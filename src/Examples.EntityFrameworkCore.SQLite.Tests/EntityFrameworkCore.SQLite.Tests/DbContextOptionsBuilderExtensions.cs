using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Examples.EntityFrameworkCore.SQLite.Tests;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder<TContext> UseSqliteDefault<TContext>(this DbContextOptionsBuilder<TContext> options,
        string? connectionString = null,
        Action<SqliteDbContextOptionsBuilder>? optionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseSqliteDefault((DbContextOptionsBuilder)options, connectionString, optionsAction);

    public static DbContextOptionsBuilder UseSqliteDefault(this DbContextOptionsBuilder options,
        string? connectionString = null,
        Action<SqliteDbContextOptionsBuilder>? optionsAction = null)
    {
        connectionString ??= @"Data Source=Examples.db";
        options.UseSqlite(connectionString, optionsAction);

        return options;
    }

    public static DbContextOptionsBuilder<TContext> UseSqliteInMemory<TContext>(this DbContextOptionsBuilder<TContext> options,
        Action<SqliteDbContextOptionsBuilder>? optionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseSqliteInMemory((DbContextOptionsBuilder)options, optionsAction);

    public static DbContextOptionsBuilder UseSqliteInMemory(this DbContextOptionsBuilder options,
        Action<SqliteDbContextOptionsBuilder>? optionsAction = null)
        => options.UseSqlite(@"Data Source=:memory:", optionsAction);

    public static DbContextOptionsBuilder<TContext> UseSqliteSharedInMemory<TContext>(this DbContextOptionsBuilder<TContext> options,
        Action<SqliteDbContextOptionsBuilder>? optionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseSqliteSharedInMemory((DbContextOptionsBuilder)options, optionsAction);

    public static DbContextOptionsBuilder UseSqliteSharedInMemory(this DbContextOptionsBuilder options,
        Action<SqliteDbContextOptionsBuilder>? optionsAction = null)
        => options.UseSqlite(@"Data Source=Sharable:Mode=Memory;Cache=Shared", optionsAction);

}
