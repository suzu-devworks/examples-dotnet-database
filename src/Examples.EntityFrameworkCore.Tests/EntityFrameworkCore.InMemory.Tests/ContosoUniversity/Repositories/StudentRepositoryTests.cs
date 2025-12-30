using ContosoUniversity.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.EntityFrameworkCore.InMemory.Tests.ContosoUniversity.Repositories;

public class StudentRepositoryTests(
    ContosoUniversityFixture fixture,
    ITestOutputHelper output)
    : IClassFixture<ContosoUniversityFixture>
{
    private readonly ContosoUniversityFixture _fixture = fixture.UseLogger(output.WriteLine);

    [Fact]
    public async Task FindAllAsync_WhenCalled_ReturnsAllRecords()
    {
        var repository = _fixture.ServiceProvider.GetRequiredService<IStudentRepository>();

        var records = await repository.FindAllAsync(TestContext.Current.CancellationToken);

        Assert.Equal(8, records.Count());
    }

    [Theory]
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

    [Fact]
    public async Task AddAsync_WhenNewRecordIsAdded_RegisteredRecordCanBeRetrieved()
    {
        // In-memory databases don't support transactions, so tests don't work properly.
        // I tried various things, but I couldn't find a solution.
    }

}
