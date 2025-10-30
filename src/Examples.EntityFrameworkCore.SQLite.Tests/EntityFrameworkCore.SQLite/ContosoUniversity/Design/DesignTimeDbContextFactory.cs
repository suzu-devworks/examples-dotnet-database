using System.Reflection;
using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Examples.EntityFrameworkCore.SQLite.ContosoUniversity.Design;

/// <summary>
/// Design-time DbContext Factory.
/// </summary>
/// <see href="https://learn.microsoft.com/ja-jp/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli" />
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SchoolContext>
{
    public SchoolContext CreateDbContext(string[] args)
    {
        var connectionString = @"Data Source=ContosoUniversity.db;Cache=Shared";
        var options = new DbContextOptionsBuilder<SchoolContext>()
            .UseSqlite(connectionString, o => o.MigrationsAssembly(Assembly.GetExecutingAssembly()))
            .Options;

        return new SchoolContext(options);
    }

}
