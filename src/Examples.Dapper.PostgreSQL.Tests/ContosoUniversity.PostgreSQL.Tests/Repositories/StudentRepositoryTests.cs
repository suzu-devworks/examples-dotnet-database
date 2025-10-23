using System.Transactions;
using ContosoUniversity.Models;
using ContosoUniversity.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.ContosoUniversity.PostgreSQL.Tests.Repositories;

public class StudentRepositoryTests(
    ContosoUniversityFixture fixture,
    ITestOutputHelper output)
    : IClassFixture<ContosoUniversityFixture>
{
    public static bool IsDBAvailable => ContosoUniversityFixture.Enabled;

    private readonly ContosoUniversityFixture _fixture = fixture.UseLogger(output.WriteLine);

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task FindAllAsync_ReturnsAllRecords()
    {
        var repository = _fixture.ServiceProvider.GetRequiredService<IStudentRepository>();

        var records = await repository.FindAllAsync(TestContext.Current.CancellationToken);

        Assert.Equal(8, records.Count());
    }

    [Theory(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    [InlineData(1, "Alexander")]
    [InlineData(2, "Alonso")]
    [InlineData(3, "Anand")]
    public async Task FindAsync_WithPrimaryKey_ReturnsAsExpected(int id, string lastName)
    {
        // spell-checker: words Anand Alonso
        var repository = _fixture.ServiceProvider.GetRequiredService<IStudentRepository>();

        var record = await repository.FindAsync(id, TestContext.Current.CancellationToken);

        Assert.True(record is not null);
        Assert.Equal(id, record.ID);
        Assert.Equal(lastName, record.LastName);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task AddAsync_WithNewData_IsAdded()
    {
        var repository = _fixture.ServiceProvider.GetRequiredService<IStudentRepository>();

        var input = new Student
        {
            FirstMidName = "Hoge",
            LastName = "Foo",
            EnrollmentDate = DateTime.Parse("2022-10-01"),
        };
        // spell-checker: words Hoge

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await repository.AddAsync(input, TestContext.Current.CancellationToken);

        var records = await repository.FindAllAsync(TestContext.Current.CancellationToken);
        Assert.Equal(9, records.Count());

        // transaction is Rollback.
    }

}
