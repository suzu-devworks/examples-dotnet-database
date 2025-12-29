using ConsoleAppFramework;
using Examples.FluentMigrator.PostgreSQL.Services;
using Microsoft.Extensions.Logging;

namespace Examples.FluentMigrator.PostgreSQL.Commands;

/// <summary>
/// Command to check RLS (Row Level Security) settings.
/// </summary>
/// <param name="migrationService"></param>
/// <param name="logger"></param>
[RegisterCommands]
public class CheckRlsCommand(
    MigrationService migrationService,
    ILogger<CheckRlsCommand> logger)
{
    /// <summary>
    /// Check the RLS (Row Level Security) setting status.
    /// </summary>
    /// <returns></returns>
    [Command("check-rls")]
    public async Task ExecuteAsync()
    {
        try
        {
            logger.LogInformation("Starting RLS check...");
            await migrationService.CheckRlsAsync();
            logger.LogInformation("RLS check completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "RLS check failed: {message}", ex.Message);
            throw;
        }
    }
}
