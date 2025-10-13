using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Examples.EntityFrameworkCore.InMemory.Tests;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder<TContext> UseInMemoryDatabaseDefault<TContext>(this DbContextOptionsBuilder<TContext> options,
        string? connectionString = null,
        Action<InMemoryDbContextOptionsBuilder>? optionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseInMemoryDatabaseDefault((DbContextOptionsBuilder)options, connectionString, optionsAction);

    public static DbContextOptionsBuilder UseInMemoryDatabaseDefault(this DbContextOptionsBuilder options,
        string? connectionString = null,
        Action<InMemoryDbContextOptionsBuilder>? optionsAction = null)
    {
        connectionString ??= "Examples.InMemory";
        options.UseInMemoryDatabase(connectionString, optionsAction)
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));

        return options;
    }

}
