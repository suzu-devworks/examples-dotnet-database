using System.Reflection;
using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Examples.EntityFrameworkCore.PostgreSQL.Tests.ContosoUniversity.Data;

/// <summary>
/// Design-time DbContext factory.
/// </summary>
/// <remarks>
/// If a class implementing this interface is found in either the same project as the derived 
/// DbContext or in the application's startup project, 
/// the tools bypass the other ways of creating the DbContext and use the design-time factory instead.
/// </remarks>
/// <see href="https://learn.microsoft.com/ja-jp/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli" />
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SchoolContext>
{
    public SchoolContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<SchoolContext>()
            .UseNpgsqlDefault(optionsAction: o => o.MigrationsAssembly(Assembly.GetExecutingAssembly()))
            .Options;

        return new SchoolContext(options);
    }
}

