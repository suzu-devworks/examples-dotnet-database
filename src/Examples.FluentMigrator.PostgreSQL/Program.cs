using System.Reflection;
using ConsoleAppFramework;
using Examples.FluentMigrator.PostgreSQL.Services;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var app = ConsoleApp.Create();

app.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Information);
    logging.AddConsole();
});

app.ConfigureServices((context, services) =>
{
    // Add Configuration as singleton.
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
        .AddUserSecrets<Program>()
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();

    services.AddSingleton<IConfiguration>(configuration);

    // Add FluentMigrator services.
    services.AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddPostgres()
            .WithGlobalConnectionString(configuration.GetConnectionString("Default"))
            .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()
        ).AddLogging(lb => lb.AddFluentMigratorConsole());

    // Add services
    services.AddSingleton<MigrationService>();
    services.AddSingleton<RlsCheckService>();
});

app.Run(args);
