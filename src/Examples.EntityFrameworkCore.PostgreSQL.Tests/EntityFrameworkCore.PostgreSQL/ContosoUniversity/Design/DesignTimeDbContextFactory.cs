using System.Reflection;
using Examples.EntityFrameworkCore.PostgreSQL.ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Examples.EntityFrameworkCore.PostgreSQL.ContosoUniversity.Design;

/// <summary>
/// Design-time DbContext Factory.
/// </summary>
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
