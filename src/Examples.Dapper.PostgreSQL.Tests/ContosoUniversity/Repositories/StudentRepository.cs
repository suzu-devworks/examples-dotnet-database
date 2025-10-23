using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Dapper;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0130
namespace ContosoUniversity.Repositories;

public class StudentRepository(
    IDbConnectionProvider dbConnectionProvider,
    ILogger<StudentRepository> logger)
    : IStudentRepository
{
    private readonly IDbConnectionProvider _dbConnectionProvider = dbConnectionProvider;
    private readonly ILogger<StudentRepository> _logger = logger;

    public async Task<IEnumerable<Student>> FindAllAsync(CancellationToken cancelToken = default)
    {
        var query = """
            SELECT id, last_name, first_mid_name, enrollment_date
            FROM "user".students;
            """;

        using var connection = _dbConnectionProvider.OpenConnection();

        var students = await connection.QueryAsync<Student>(
              new CommandDefinition(query,
                commandTimeout: _dbConnectionProvider.CommandTimeout,
                cancellationToken: cancelToken));

        _logger?.LogDebug("Students is founds: count=\"{count}\".", students.Count());

        return [.. students];
    }

    public async Task<Student?> FindAsync(int id, CancellationToken cancelToken = default)
    {
        var query = """
            SELECT id, last_name, first_mid_name, enrollment_date
            FROM "user".students
            WHERE id = @id;
            """;

        using var connection = _dbConnectionProvider.OpenConnection();
        var student = await connection.QuerySingleOrDefaultAsync<Student>(
            new CommandDefinition(query,
                parameters: new { id },
                commandTimeout: _dbConnectionProvider.CommandTimeout,
                cancellationToken: cancelToken)
        );

        if (student is null)
        {
            _logger.LogDebug("Students is not found.");
        }
        else
        {
            _logger.LogDebug("Students is found.");
        }

        return student;
    }

    public async Task AddAsync(Student student, CancellationToken cancelToken = default)
    {
        var query = """
            INSERT INTO "user".students (id, last_name, first_mid_name, enrollment_date)
            VALUES (@ID, @LastName, @FirstMidName, @EnrollmentDate);
            """;

        using var connection = _dbConnectionProvider.OpenConnection();
        var effectiveRows = await connection.ExecuteAsync(
            new CommandDefinition(query,
                parameters: student,
                commandTimeout: _dbConnectionProvider.CommandTimeout,
                cancellationToken: cancelToken));

        _logger.LogDebug("INSERT was successful. {effectiveRows} results.", effectiveRows);
    }

    public async Task UpdateAsync(int id, Student modifier, CancellationToken cancelToken = default)
    {
        if (id != modifier.ID)
        {
            throw new InvalidOperationException($"Invalid Student: id=\"{id}\".");
        }

        var query = """
            UPDATE "user".students
            SET last_name = @LastName, first_mid_name = @FirstMidName,  enrollment_date
            WHERE id = @ID;
            """;

        using var connection = _dbConnectionProvider.OpenConnection();
        var effectiveRows = await connection.ExecuteAsync(
            new CommandDefinition(query,
                parameters: modifier,
                commandTimeout: _dbConnectionProvider.CommandTimeout,
                cancellationToken: cancelToken));

        _logger.LogDebug("UPDATE was successful. {effectiveRows} results.", effectiveRows);
    }

    public async Task RemoveAsync(int id, CancellationToken cancelToken = default)
    {
        var query = """
            DELETE FROM user.students
            WHERE id = @ID
            """;

        using var connection = _dbConnectionProvider.OpenConnection();
        var effectiveRows = await connection.ExecuteAsync(
            new CommandDefinition(query,
                parameters: new { id },
                commandTimeout: _dbConnectionProvider.CommandTimeout,
                cancellationToken: cancelToken));

        _logger.LogDebug("DELETE was successful. {effectiveRows} results.", effectiveRows);
    }

}
