using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Examples.ContosoUniversity.PostgreSQL.Data;

/// <summary>
/// Design-time DbContext factory.
/// </summary>
/// <remarks>
/// If a class implementing this interface is found in either the same project as the derived
/// DbContext or in the application's startup project,
/// the tools bypass the other ways of creating the DbContext and use the design-time factory instead.
/// </remarks>
/// <see href="https://learn.microsoft.com/ja-jp/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli" />
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NpgsqlSchoolContext>
{
    public NpgsqlSchoolContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddUserSecrets<DesignTimeDbContextFactory>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("ContosoUniversity")
            ?? throw new InvalidOperationException("ConnectionStrings:ContosoUniversity is required.");

        var options = new DbContextOptionsBuilder<NpgsqlSchoolContext>()
            .UseNpgsql(connectionString, o => o.MigrationsAssembly(Assembly.GetExecutingAssembly()))
            .Options;

        return new NpgsqlSchoolContext(options);
    }

}
