using System.Transactions;
using ContosoUniversity.Abstraction;
using ContosoUniversity.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL.Tests.ContosoUniversity.Repositories;

public class StudentRepositoryTests(
    ContosoUniversityFixture fixture,
    ITestOutputHelper output)
    : IClassFixture<ContosoUniversityFixture>
{
    public static bool IsDBAvailable => DatabaseEnvironment.IsAvailable;

    private readonly ContosoUniversityFixture _fixture = fixture.UseLogger(output.WriteLine);

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task FindAllAsync_WhenCalled_ReturnsAllRecords()
    {
        var repository = _fixture.ServiceProvider.GetRequiredService<IStudentRepository>();

        var records = await repository.FindAllAsync(TestContext.Current.CancellationToken);

        Assert.Equal(8, records.Count());
    }

    [Theory(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    [InlineData(1, "Alexander")]
    [InlineData(2, "Alonso")]
    [InlineData(3, "Anand")]
    public async Task FindAsync_WhenPrimaryKeyIsProvided_ReturnsSpecifiedRecord(int id, string lastName)
    {
        // spell-checker: words Anand Alonso
        var repository = _fixture.ServiceProvider.GetRequiredService<IStudentRepository>();

        var record = await repository.FindAsync(id, TestContext.Current.CancellationToken);

        Assert.True(record is not null);
        Assert.Equal(id, record.ID);
        Assert.Equal(lastName, record.LastName);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task AddAsync_WhenNewRecordIsAdded_RegisteredRecordCanBeRetrieved()
    {
        var repository = _fixture.ServiceProvider.GetRequiredService<IStudentRepository>();

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var input = new Student
        {
            FirstMidName = "Hoge",
            LastName = "Foo",
            EnrollmentDate = DateTime.Parse("2022-10-01"),
        };
        // spell-checker: words Hoge

        await repository.AddAsync(input, TestContext.Current.CancellationToken);

        var records = await repository.FindAllAsync(TestContext.Current.CancellationToken);
        Assert.Equal(9, records.Count());
    }

}
