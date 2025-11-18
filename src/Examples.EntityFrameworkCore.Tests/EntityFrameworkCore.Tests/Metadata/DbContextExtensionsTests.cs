using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Examples.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Examples.EntityFrameworkCore.Tests.Metadata;

public class DbContextExtensionsTests : IDisposable
{
    private readonly SchoolContext _context;

    public DbContextExtensionsTests()
    {
        var options = new DbContextOptionsBuilder<SchoolContext>()
            .UseInMemoryDatabase(nameof(DbContextExtensionsTests))
            .Options;

        _context = new SchoolContext(options);
    }

    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }

    [Theory]
    [InlineData(typeof(Student), "Students")]
    [InlineData(typeof(Enrollment), "Enrollments")]
    [InlineData(typeof(Course), "Courses")]
    public void GetTableName_WhenTypeIsSpecified_ReturnsPluralizedTableName(Type modelType, string expected)
    {
        Assert.Equal(expected, _context.GetTableName(modelType));
    }

    [Fact]
    public void GetTableName_WhenGenericTypeAndInstanceProvided_ReturnsPluralizedTableName()
    {
        Assert.Equal("Students", _context.GetTableName(new Student()));
        Assert.Equal("Enrollments", _context.GetTableName(new Enrollment()));
        Assert.Equal("Courses", _context.GetTableName(new Course()));
    }

}
