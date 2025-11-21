using System.Data.Common;
using ContosoUniversity.Abstraction;
using ContosoUniversity.Models;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Examples.Dapper.PostgreSQL.ContosoUniversity.Repositories;

public class StudentRepository(
    [FromKeyedServices(DataSourceKeys.ContosoUniversity)] DbDataSource dataSource,
    ILogger<StudentRepository> logger)
    : IStudentRepository
{
    private readonly DbDataSource _dbDataSource = dataSource;
    private readonly ILogger<StudentRepository> _logger = logger;

    public async Task<IEnumerable<Student>> FindAllAsync(CancellationToken cancelToken = default)
    {
        using var connection = await _dbDataSource.OpenConnectionAsync(cancelToken);

        var query = """
            SELECT id, last_name, first_mid_name, enrollment_date
            FROM "user".students;
            """;
        var students = await connection.QueryAsync<Student>(
              new CommandDefinition(query,
                cancellationToken: cancelToken));

        _logger?.LogDebug("Students is founds: count=\"{count}\".", students.Count());

        return [.. students];
    }

    public async Task<Student?> FindAsync(int id, CancellationToken cancelToken = default)
    {
        using var connection = await _dbDataSource.OpenConnectionAsync(cancelToken);

        var query = """
            SELECT id, last_name, first_mid_name, enrollment_date
            FROM "user".students
            WHERE id = @id;
            """;
        var student = await connection.QuerySingleOrDefaultAsync<Student>(
            new CommandDefinition(query,
                parameters: new { id },
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
        using var connection = await _dbDataSource.OpenConnectionAsync(cancelToken);

        var query = """
            INSERT INTO "user".students (id, last_name, first_mid_name, enrollment_date)
            VALUES (@ID, @LastName, @FirstMidName, @EnrollmentDate);
            """;
        var effectiveRows = await connection.ExecuteAsync(
            new CommandDefinition(query,
                parameters: student,
                cancellationToken: cancelToken));

        _logger.LogDebug("INSERT was successful. {effectiveRows} results.", effectiveRows);
    }

    public async Task UpdateAsync(int id, Student modifier, CancellationToken cancelToken = default)
    {
        if (id != modifier.ID)
        {
            throw new InvalidOperationException($"Invalid Student: id=\"{id}\".");
        }

        using var connection = await _dbDataSource.OpenConnectionAsync(cancelToken);

        var query = """
            UPDATE "user".students
            SET last_name = @LastName, first_mid_name = @FirstMidName,  enrollment_date
            WHERE id = @ID;
            """;
        var effectiveRows = await connection.ExecuteAsync(
            new CommandDefinition(query,
                parameters: modifier,
                cancellationToken: cancelToken));

        _logger.LogDebug("UPDATE was successful. {effectiveRows} results.", effectiveRows);
    }

    public async Task RemoveAsync(int id, CancellationToken cancelToken = default)
    {
        using var connection = await _dbDataSource.OpenConnectionAsync(cancelToken);

        var query = """
            DELETE FROM user.students
            WHERE id = @ID
            """;
        var effectiveRows = await connection.ExecuteAsync(
            new CommandDefinition(query,
                parameters: new { id },
                cancellationToken: cancelToken));

        _logger.LogDebug("DELETE was successful. {effectiveRows} results.", effectiveRows);
    }

}
