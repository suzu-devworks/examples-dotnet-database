using ConsoleAppFramework;
using Examples.FluentMigrator.PostgreSQL.Services;
using Microsoft.Extensions.Logging;

namespace Examples.FluentMigrator.PostgreSQL.Commands;

[RegisterCommands]
public class InfoCommand(
    MigrationService migrationService,
    ILogger<CheckRlsCommand> logger)
{
    [Command("info")]
    public async Task ExecuteAsync()
    {
        try
        {
            var version = await migrationService.GetLatestVersionAsync();
            logger.LogInformation("Latest migration version: {version}", version);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get latest migration version: {message}", ex.Message);
            throw;
        }
    }
}
