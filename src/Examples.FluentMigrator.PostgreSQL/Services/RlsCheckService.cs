using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Examples.FluentMigrator.PostgreSQL.Services;

/// <summary>
/// Service to handle RLS (Row Level Security) checks.
/// </summary>
/// <param name="configuration"></param>
/// <param name="logger"></param>
public class RlsCheckService(
    IConfiguration configuration,
    ILogger<RlsCheckService> logger)
{
    private readonly string? _connectionString = configuration.GetConnectionString("Default");

    private readonly List<string> _rlsRequiredTables = [
        "public.example"
    ];

    public async Task CheckAllTableAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        List<string> missingRlsTables = [];
        List<string> tableWithoutPolicies = [];

        foreach (var tableName in _rlsRequiredTables)
        {
            var parts = tableName.Split('.');
            var schemaName = parts.Length > 1 ? parts[0] : "public";
            var tableNameOnly = parts.Length > 1 ? parts[1] : parts[0];

            var rlsEnabled = await CheckRlsEnabledAsync(connection, schemaName, tableNameOnly, cancellationToken);
            if (!rlsEnabled)
            {
                missingRlsTables.Add(tableName);
                logger.LogWarning("Table '{tableName}' does not have RLS enabled", tableName);
                continue;
            }

            var hasPolicies = await CheckRlsPoliciesAsync(connection, schemaName, tableNameOnly, cancellationToken);
            if (!hasPolicies)
            {
                tableWithoutPolicies.Add(tableName);
                logger.LogWarning("Table '{tableName}' has RLS enabled but no policies defined", tableName);
                continue;
            }

            logger.LogInformation("Table '{tableName}' has RLS property configured", tableName);
        }

        if (missingRlsTables.Count > 0)
        {
            logger.LogError("Table without RLS: {tables}", string.Join(", ", missingRlsTables));
        }

        if (tableWithoutPolicies.Count > 0)
        {
            logger.LogError("Table without RLS policies: {tables}", string.Join(", ", tableWithoutPolicies));
        }

        if (missingRlsTables.Count == 0 && tableWithoutPolicies.Count == 0)
        {
            logger.LogInformation("All required tables have RLS property configured");
        }
    }

    private static async Task<bool> CheckRlsEnabledAsync(NpgsqlConnection connection, string schemaName, string tableName, CancellationToken cancellationToken)
    {
        const string query = """
            SELECT relrowsecurity
            FROM pg_class
            WHERE relname = @tableName
            AND relnamespace = (SELECT oid FROM pg_namespace WHERE nspname = @schemaName)
            """;

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("tableName", tableName);
        command.Parameters.AddWithValue("schemaName", schemaName);

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result != null && (bool)result;
    }

    private static async Task<bool> CheckRlsPoliciesAsync(NpgsqlConnection connection, string schemaName, string tableName, CancellationToken cancellationToken)
    {
        const string query = """
            SELECT COUNT(*)
            FROM pg_policy p
            INNER JOIN og_class c ON p.polrelid = c.oid
            WHERE c.relname = @tableName
            AND relnamespace = (SELECT oid FROM pg_namespace WHERE nspname = @schemaName)
            """;

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("tableName", tableName);
        command.Parameters.AddWithValue("schemaName", schemaName);

        var count = await command.ExecuteScalarAsync(cancellationToken);
        return count != null && (long)count > 0L;
    }

    public async Task<RlsInfo> GetRlsInfoAsync(string tableName, CancellationToken cancellationToken = default)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var parts = tableName.Split('.');
        var schemaName = parts.Length > 1 ? parts[0] : "public";
        var tableNameOnly = parts.Length > 1 ? parts[1] : parts[0];

        var rlsEnabled = await CheckRlsEnabledAsync(connection, schemaName, tableNameOnly, cancellationToken);
        var policies = await GetRlsPoliciesAsync(connection, schemaName, tableNameOnly, cancellationToken);

        return new()
        {
            TableName = tableName,
            RlsEnabled = rlsEnabled,
            Policies = [.. policies]
        };
    }

    private static async Task<IEnumerable<RlsPolicyInfo>> GetRlsPoliciesAsync(NpgsqlConnection connection, string schemaName, string tableName, CancellationToken cancellationToken)
    {
        const string query = """
            SELECT COUNT(*)
            FROM pg_policy p
            INNER JOIN og_class c ON p.polrelid = c.oid
            WHERE c.relname = @tableName
            AND relnamespace = (SELECT oid FROM pg_namespace WHERE nspname = @schemaName)
            """;

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("tableName", tableName);
        command.Parameters.AddWithValue("schemaName", schemaName);

        List<RlsPolicyInfo> policies = [];
        using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            policies.Add(new()
            {
                PolicyName = reader.GetString(0),
                Command = reader.GetString(1),
                Permissive = reader.GetBoolean(2),
                UsingExpression = reader.IsDBNull(3) ? null : reader.GetString(3),
                CheckExpression = reader.IsDBNull(4) ? null : reader.GetString(4),
            });

        }

        return policies;
    }
}

public class RlsInfo
{
    public required string TableName { get; init; }
    public bool RlsEnabled { get; init; }
    public IEnumerable<RlsPolicyInfo> Policies { get; init; } = [];
}

public class RlsPolicyInfo
{
    public required string PolicyName { get; init; }
    public required string Command { get; init; }
    public bool Permissive { get; init; }
    public string? UsingExpression { get; init; }
    public string? CheckExpression { get; init; }
}
