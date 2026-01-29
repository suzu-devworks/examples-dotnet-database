using ContosoUniversity;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.EntityFrameworkCore.SQLite.Tests.ContosoUniversity.Tutorials;

/// <summary>
/// Sort, Filter, Paging.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/sort-filter-page"/>
public class SortFilterPagingTests(
    ContosoUniversityFixture fixture,
    ITestOutputHelper output)
    : IClassFixture<ContosoUniversityFixture>
{
    private readonly ContosoUniversityFixture _fixture = fixture.UseLogger(output.WriteLine);

    [Theory]
    [InlineData(null, "Alexander", "2019-09-01")]
    [InlineData("Name", "Alexander", "2019-09-01")]
    [InlineData("name_desc", "Olivetto", "2019-09-01")]
    [InlineData("Date", "Justice", "2016-09-01")]
    [InlineData("date_desc", "Alexander", "2019-09-01")]
    public async Task When_UseIQueryableOrderBy_Then_ReturnsSortedResults(string? sortOrder, string expectedName, string expectedDate)
    {
        var context = _fixture.ServiceProvider.GetRequiredService<SchoolContext>();

        IQueryable<Student> studentsIQ = from s in context.Students
                                         select s;
        studentsIQ = sortOrder switch
        {
            "name_desc" => studentsIQ.OrderByDescending(s => s.LastName),
            "Date" => studentsIQ.OrderBy(s => s.EnrollmentDate),
            "date_desc" => studentsIQ.OrderByDescending(s => s.EnrollmentDate),
            _ => studentsIQ.OrderBy(s => s.LastName),
        };

        var students = await studentsIQ.AsNoTracking().ToListAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotEmpty(students);
        Assert.Equal(expectedName, students[0].LastName);
        Assert.Equal(DateTime.Parse(expectedDate), students[0].EnrollmentDate);
    }

    [Theory]
    [InlineData("and", 2)]
    [InlineData("i", 5)]
    [InlineData("Nino", 1)]
    public async Task When_UseIQueryableWhere_Then_ReturnsFilteredResults(string? searchString, int expectedCount)
    {
        var context = _fixture.ServiceProvider.GetRequiredService<SchoolContext>();

        IQueryable<Student> studentsIQ = from s in context.Students
                                         select s;

        if (!string.IsNullOrEmpty(searchString))
        {
            // There's a performance penalty for calling ToUpper. 
            // The ToUpper code adds a function in the WHERE clause of the TSQL SELECT statement.
            studentsIQ = studentsIQ.Where(s => s.LastName != null && s.LastName.Contains(searchString)
                                   || s.FirstMidName != null && s.FirstMidName.Contains(searchString));
        }

        var students = await studentsIQ.AsNoTracking().ToListAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(expectedCount, students.Count);
    }

    [Theory]
    [InlineData(null, 4, "Alexander")]
    [InlineData(1, 4, "Alexander")]
    [InlineData(2, 4, "Justice")]
    [InlineData(3, 0, null)]
    public async Task When_UseIQueryableWithPaginatedList_Then_ReturnsPaginationResults(int? pageIndex, int expectedCount, string? expectedName)
    {
        var context = _fixture.ServiceProvider.GetRequiredService<SchoolContext>();
        var configuration = _fixture.ServiceProvider.GetRequiredService<IConfiguration>();
        var pageSize = configuration.GetValue("PageSize", 4);

        IQueryable<Student> studentsIQ = from s in context.Students
                                         select s;
        studentsIQ = studentsIQ.OrderBy(s => s.LastName);

        var students = await PaginatedList<Student>.CreateAsync(
            studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

        Assert.Equal(expectedCount, students.Count);
        if (expectedCount > 0)
        {
            Assert.Equal(expectedName, students[0].LastName);
        }
    }

    [Fact]
    public async Task When_UseIQueryableGroupBy_Then_ReturnsGroupingResults()
    {
        var context = _fixture.ServiceProvider.GetRequiredService<SchoolContext>();

        IQueryable<EnrollmentDateGroup> data =
                      from student in context.Students
                      group student by student.EnrollmentDate into dateGroup
                      select new EnrollmentDateGroup()
                      {
                          EnrollmentDate = dateGroup.Key,
                          StudentCount = dateGroup.Count()
                      };

        var students = await data.AsNoTracking().ToListAsync(cancellationToken: TestContext.Current.CancellationToken);

        Assert.Collection(students.OrderBy(x => x.EnrollmentDate),
            (s) =>
            {
                Assert.Equal(DateTime.Parse("2016-09-01"), s.EnrollmentDate);
                Assert.Equal(1, s.StudentCount);
            },
            (s) =>
            {
                Assert.Equal(DateTime.Parse("2017-09-01"), s.EnrollmentDate);
                Assert.Equal(3, s.StudentCount);
            },
            (s) =>
            {
                Assert.Equal(DateTime.Parse("2018-09-01"), s.EnrollmentDate);
                Assert.Equal(2, s.StudentCount);
            },
            (s) =>
            {
                Assert.Equal(DateTime.Parse("2019-09-01"), s.EnrollmentDate);
                Assert.Equal(2, s.StudentCount);
            }
        );
    }

}
