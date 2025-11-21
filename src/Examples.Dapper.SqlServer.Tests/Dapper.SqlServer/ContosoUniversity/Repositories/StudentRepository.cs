using System.Data.Common;
using ContosoUniversity.Abstraction;
using ContosoUniversity.Models;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Examples.Dapper.SqlServer.ContosoUniversity.Repositories;

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
            SELECT ID, LastName, FirstMidName, EnrollmentDate
            FROM "user".Students;
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
            SELECT ID, LastName, FirstMidName, EnrollmentDate
            FROM "user".Students
            WHERE ID = @id;
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
            SET IDENTITY_INSERT "user".Students ON;
            INSERT INTO "user".Students (ID, LastName, FirstMidName, EnrollmentDate)
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
            UPDATE "user".Students
            SET LastName = @LastName, FirstMidName = @FirstMidName
            WHERE ID = @ID;
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
            DELETE FROM user.Students
            WHERE ID = @ID
            """;
        var effectiveRows = await connection.ExecuteAsync(
            new CommandDefinition(query,
                parameters: new { id },
                cancellationToken: cancelToken));

        _logger.LogDebug("DELETE was successful. {effectiveRows} results.", effectiveRows);
    }

}
