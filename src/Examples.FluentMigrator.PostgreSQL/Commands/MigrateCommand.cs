using ConsoleAppFramework;
using Examples.FluentMigrator.PostgreSQL.Services;
using Microsoft.Extensions.Logging;

namespace Examples.FluentMigrator.PostgreSQL.Commands;

/// <summary>
///  Command to run database migrations.
/// </summary>
/// <param name="migrationService"></param>
/// <param name="logger"></param>
[RegisterCommands]
public class MigrateCommand(
    MigrationService migrationService,
    ILogger<MigrateCommand> logger)
{
    /// <summary>
    /// Run the database migration.
    /// </summary>
    /// <param name="withRlsCheck">-r, Whether to run an RLS check after migration.</param>
    /// <param name="version">-v, The specific version to migrate to. If not provided, migrates to the latest version.</param>
    /// <returns></returns>
    [Command("migrate")]
    public async Task ExecuteAsync(bool withRlsCheck = true, long? version = null)
    {
        try
        {
            logger.LogInformation("Starting database migration...");

            if (version is not null)
            {
                logger.LogInformation("Migrating to version {version}...", version.Value);
                await migrationService.RunMigrationVersionAsync(version.Value);
            }
            else
            {
                logger.LogInformation("Migrating to the latest version...");
                await migrationService.RunMigrationAsync();
            }

            if (withRlsCheck)
            {
                logger.LogInformation("Running RLS check...");
                await migrationService.CheckRlsAsync();
            }

            logger.LogInformation("Migration completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Migration failed: {message}", ex.Message);
            throw;
        }
    }

}
