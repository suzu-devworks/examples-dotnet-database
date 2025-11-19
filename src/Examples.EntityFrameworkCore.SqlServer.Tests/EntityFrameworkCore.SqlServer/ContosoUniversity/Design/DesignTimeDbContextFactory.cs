using System.Reflection;
using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Examples.EntityFrameworkCore.SqlServer.ContosoUniversity.Design;

/// <summary>
/// Design-time DbContext Factory.
/// </summary>
/// <see href="https://learn.microsoft.com/ja-jp/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli" />
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SchoolContext>
{
    public SchoolContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddUserSecrets<DesignTimeDbContextFactory>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("ContosoUniversity")
            ?? throw new InvalidOperationException("ConnectionStrings:ContosoUniversity is required.");

        var options = new DbContextOptionsBuilder<SchoolContext>()
            .UseSqlServer(connectionString, o => o.MigrationsAssembly(Assembly.GetExecutingAssembly()))
            .Options;

        return new SchoolContext(options);
    }

}
