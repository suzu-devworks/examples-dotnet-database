using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Repositories;
using Examples.EntityFrameworkCore.PostgreSQL.Tests.ContosoUniversity.Data;
using Examples.Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.EntityFrameworkCore.PostgreSQL.Tests.ContosoUniversity.Repositories;

public class StudentRepositoryTests : IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IStudentRepository _repository;

    public StudentRepositoryTests(ITestOutputHelper testOutputHelper)
    {
        var services = new ServiceCollection();
        services.AddLogging(builder
            => builder.AddXunit(testOutputHelper));

        services.AddDbContext<SchoolContext, NpgsqlSchoolContext>(options
            => options.UseNpgsqlDefault());

        services.AddScoped<IStudentRepository, StudentRepository>();

        _serviceProvider = services.BuildServiceProvider();

        InitializeDatabase();

        _repository = _serviceProvider.GetRequiredService<IStudentRepository>();

    }

    private void InitializeDatabase()
    {
        var context = _serviceProvider.GetRequiredService<SchoolContext>();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInitializer.Initialize(context);

        context.SaveChanges();
    }

    public void Dispose()
    {
        (_serviceProvider as IDisposable)?.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task FindAllAsync_ReturnsAllRecords()
    {
        var records = await _repository.FindAllAsync();

        Assert.Equal(8, records.Count());
    }

    [Theory]
    [InlineData(1, "Alexander")]
    [InlineData(2, "Alonso")]
    [InlineData(3, "Anand")]
    public async Task FindAsync_WithPrimaryKey_ReturnsAsExpected(int id, string lastName)
    {
        // spell-checker: words Anand Alonso

        var record = await _repository.FindAsync(id);

        Assert.True(record is not null);
        Assert.Equal(id, record.ID);
        Assert.Equal(lastName, record.LastName);
    }

    [Fact]
    public async Task AddAsync_WithNewData_IsAdded()
    {
        var input = new Student
        {
            FirstMidName = "Hoge",
            LastName = "Foo",
            EnrollmentDate = DateTime.Parse("2022-10-01"),
        };
        // spell-checker: words Hoge

        await _repository.AddAsync(input);

        var records = await _repository.FindAllAsync();
        Assert.Equal(9, records.Count());
    }

}
