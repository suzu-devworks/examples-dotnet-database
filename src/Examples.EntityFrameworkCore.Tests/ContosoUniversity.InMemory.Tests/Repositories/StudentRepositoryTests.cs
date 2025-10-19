using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.ContosoUniversity.InMemory.Tests.Repositories;

public class StudentRepositoryTests(
    ContosoUniversityFixture fixture,
    ITestOutputHelper output)
    : IClassFixture<ContosoUniversityFixture>
{
    private readonly ContosoUniversityFixture _fixture = fixture.UseLogger(output.WriteLine);

    [Fact]
    public async Task FindAllAsync_ReturnsAllRecords()
    {
        var repository = _fixture.ServiceProvider.GetRequiredService<IStudentRepository>();

        var records = await repository.FindAllAsync();

        Assert.Equal(8, records.Count());
    }

    [Theory]
    [InlineData(1, "Alexander")]
    [InlineData(2, "Alonso")]
    [InlineData(3, "Anand")]
    public async Task FindAsync_WithPrimaryKey_ReturnsAsExpected(int id, string lastName)
    {
        // spell-checker: words Anand Alonso
        var repository = _fixture.ServiceProvider.GetRequiredService<IStudentRepository>();

        var record = await repository.FindAsync(id);

        Assert.True(record is not null);
        Assert.Equal(id, record.ID);
        Assert.Equal(lastName, record.LastName);
    }

    [Fact]
    public async Task AddAsync_WithNewData_IsAdded()
    {
        using var scoped = _fixture.ServiceProvider.CreateScope();
        var context = scoped.ServiceProvider.GetRequiredService<SchoolContext>();
        await context.Database.BeginTransactionAsync();

        var repository = scoped.ServiceProvider.GetRequiredService<IStudentRepository>();

        var input = new Student
        {
            FirstMidName = "Hoge",
            LastName = "Foo",
            EnrollmentDate = DateTime.Parse("2022-10-01")
        };
        // spell-checker: words Hoge

        await repository.AddAsync(input);

        context.ChangeTracker.Clear();

        var records = await repository.FindAllAsync();
        Assert.Equal(9, records.Count());
    }

}
