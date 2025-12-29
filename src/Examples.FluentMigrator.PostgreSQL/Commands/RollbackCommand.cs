using ConsoleAppFramework;
using Examples.FluentMigrator.PostgreSQL.Services;
using Microsoft.Extensions.Logging;

namespace Examples.FluentMigrator.PostgreSQL.Commands;

/// <summary>
/// Command to roll back database migrations.
/// </summary>
/// <param name="migrationService"></param>
/// <param name="logger"></param>
[RegisterCommands]
public class RollbackCommand(
    MigrationService migrationService,
    ILogger<RollbackCommand> logger)
{
    /// <summary>
    /// Rolls back to the specified version.
    /// </summary>
    /// <param name="version">The version number to roll back to.</param>
    /// <returns></returns>
    [Command("rollback")]
    public async Task ExecuteAsync(long version)
    {
        try
        {
            logger.LogInformation("Starting rollback to version {version}...", version);
            await migrationService.RollbackToVersionAsync(version);
            logger.LogInformation("Rollback completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Rollback failed: {message}", ex.Message);
            throw;
        }
    }

}
