using System.Reflection;
using ConsoleAppFramework;
using Examples.FluentMigrator.PostgreSQL.Migrations;
using Examples.FluentMigrator.PostgreSQL.Services;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var app = ConsoleApp.Create();

app.ConfigureGlobalOptions((ref ConsoleApp.GlobalOptionsBuilder builder) =>
{
    var connection = builder.AddGlobalOption<string>("-c|--connection", "connection string");
    // return value stored to ConsoleAppContext.GlobalOptions
    return new GlobalOptions(connection);
});

app.ConfigureLogging(logging => logging
        .ClearProviders()
        .SetMinimumLevel(LogLevel.Information)
        .AddConsole()
        );

app.ConfigureServices((context, con2, services) =>
{
    var globalOptions = (GlobalOptions)context.GlobalOptions!;

    // Add Configuration as singleton.
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
        .AddUserSecrets<Program>()
        .AddEnvironmentVariables()
        .AddInMemoryCollection(globalOptions.ToDictionary())
        .Build();
    services.AddSingleton<IConfiguration>(configuration);

    // Add FluentMigrator services.
    services.AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddPostgres()
            .WithGlobalConnectionString(configuration.GetConnectionString("Default"))
            .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()
        ).AddLogging(lb => lb.AddFluentMigratorConsole());
    services.AddSingleton<IVersionTableMetaData, CustomVersionTableMetaData>();

    // Add services
    services.AddSingleton<MigrationService>();
    services.AddSingleton<RlsCheckService>();
});

app.Run(args);

internal record GlobalOptions(string ConnectionString)
{
    public IDictionary<string, string?> ToDictionary()
    {
        var dict = new Dictionary<string, string?>();

        if (!string.IsNullOrEmpty(ConnectionString))
        {
            dict.Add("ConnectionStrings:Default", ConnectionString);
        }

        return dict;
    }
}
