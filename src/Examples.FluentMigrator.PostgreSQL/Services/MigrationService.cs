using FluentMigrator.Runner;
using Microsoft.Extensions.Logging;

namespace Examples.FluentMigrator.PostgreSQL.Services;

/// <summary>
/// Service to handle database migrations and RLS checks.
/// </summary>
/// <param name="migrationRunner"></param>
/// <param name="rlsCheckService"></param>
/// <param name="logger"></param>
public class MigrationService(
    IMigrationRunner migrationRunner,
    RlsCheckService rlsCheckService,
    ILogger<MigrationService> logger)
{
    /// <summary>
    /// Runs the database migrations.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task RunMigrationAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Running database migrations...");

        try
        {
            await Task.Run(() => migrationRunner.MigrateUp(), cancellationToken);
            logger.LogInformation("Database migrations applied.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to run migrations");
            throw;
        }
    }

    /// <summary>
    /// Runs the database migrations to a specific version.
    /// </summary>
    /// <param name="version"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task RunMigrationVersionAsync(long version, CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Running database migrations to version {version}...", version);

        try
        {
            await Task.Run(() => migrationRunner.MigrateUp(version), cancellationToken);
            logger.LogInformation("Database migrated to version {version}.", version);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to migrate to version {version}", version);
            throw;
        }
    }

    /// <summary>
    /// Rolls back the database to a specific version.
    /// </summary>
    /// <param name="version"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task RollbackToVersionAsync(long version, CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Rolling back to version {version}...", version);

        try
        {
            await Task.Run(() => migrationRunner.MigrateDown(version), cancellationToken);
            logger.LogInformation("Rolled back to version {version}.", version);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to rollback to version {version}", version);
            throw;
        }
    }

    /// <summary>
    /// Checks the RLS (Row Level Security) settings.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task CheckRlsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Checking Row Level Security settings...");

        try
        {
            await rlsCheckService.CheckAllTableAsync(cancellationToken);
            logger.LogInformation("RLS check completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "RLS check failed.");
            throw;
        }
    }

}
