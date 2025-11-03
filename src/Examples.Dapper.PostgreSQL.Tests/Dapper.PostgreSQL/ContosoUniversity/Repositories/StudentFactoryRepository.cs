using ContosoUniversity.Abstraction;
using ContosoUniversity.Models;
using Dapper;
using Examples.Dapper.PostgreSQL.Data;
using Microsoft.Extensions.Logging;

#pragma warning disable CS0618

namespace Examples.Dapper.PostgreSQL.ContosoUniversity.Repositories;

public class StudentFactoryRepository(
    IDbConnectionFactory dbConnectionFactory,
    ILogger<StudentFactoryRepository> logger)
    : IStudentRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
    private readonly ILogger<StudentFactoryRepository> _logger = logger;

    public async Task<IEnumerable<Student>> FindAllAsync(CancellationToken cancelToken = default)
    {
        var query = """
            SELECT id, last_name, first_mid_name, enrollment_date
            FROM "user".students;
            """;

        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancelToken);

        var students = await connection.QueryAsync<Student>(
              new CommandDefinition(query,
                commandTimeout: 100,
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

        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancelToken);

        var student = await connection.QuerySingleOrDefaultAsync<Student>(
            new CommandDefinition(query,
                parameters: new { id },
                commandTimeout: 100,
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

        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancelToken);

        var effectiveRows = await connection.ExecuteAsync(
            new CommandDefinition(query,
                parameters: student,
                commandTimeout: 100,
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

        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancelToken);

        var effectiveRows = await connection.ExecuteAsync(
            new CommandDefinition(query,
                parameters: modifier,
                commandTimeout: 100,
                cancellationToken: cancelToken));

        _logger.LogDebug("UPDATE was successful. {effectiveRows} results.", effectiveRows);
    }

    public async Task RemoveAsync(int id, CancellationToken cancelToken = default)
    {
        var query = """
            DELETE FROM user.students
            WHERE id = @ID
            """;

        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancelToken);

        var effectiveRows = await connection.ExecuteAsync(
            new CommandDefinition(query,
                parameters: new { id },
                commandTimeout: 100,
                cancellationToken: cancelToken));

        _logger.LogDebug("DELETE was successful. {effectiveRows} results.", effectiveRows);
    }

}
