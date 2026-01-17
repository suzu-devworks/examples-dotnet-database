using System.Linq.Expressions;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.EntityFrameworkCore.SQLite.Tests.ContosoUniversity.Tutorials;

/// <summary>
/// CRUD (create, read, update, delete).
/// </summary>
/// <param name="output"></param>
/// <seealso href="https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/crud"/>
public class CURDTests(
    ContosoUniversityFixture fixture,
    ITestOutputHelper output)
    : IClassFixture<ContosoUniversityFixture>
{
    private readonly ContosoUniversityFixture _fixture = fixture.UseLogger(output.WriteLine);

    [Fact]
    public async Task When_UsedNavigationProperty_Then_ReadsEnrollments()
    {
        var context = _fixture.ServiceProvider.GetRequiredService<SchoolContext>();

        var id = 2;

        var student = await context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id, TestContext.Current.CancellationToken);

        Assert.NotNull(student);
        Assert.NotNull(student.LastName);
        Assert.NotNull(student.FirstMidName);
        Assert.NotNull(student.EnrollmentDate);

        Assert.NotEmpty(student.Enrollments);
        foreach (var e in student.Enrollments)
        {
            Assert.NotNull(e.Course?.Title);
            Assert.NotNull(e.Grade);
        }
    }

    [Fact]
    public async Task When_EntityStateIsAdded_Then_StudentIsCreated()
    {
        using var scoped = _fixture.ServiceProvider.CreateScope();
        var context = scoped.ServiceProvider.GetRequiredService<SchoolContext>();
        using var transaction = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var emptyStudent = new Student();

        EntityState? stateBeforeSave = null;

        if (await TryUpdateModelAsync<Student>(
                emptyStudent,
                "student",   // Prefix for form value.
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
        {
            context.Students.Add(emptyStudent);

            stateBeforeSave = context.Entry(emptyStudent).State;
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        Assert.Equal(EntityState.Added, stateBeforeSave);
        Assert.True(emptyStudent.ID > 0); // updated model ID.

        context.ChangeTracker.Clear();
        var student = await context.Students.FindAsync([emptyStudent.ID], cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(student);
        Assert.Equal("Joe", student.FirstMidName);
        Assert.Equal("Smith", student.LastName);
    }

    [Fact]
    public async Task When_EntityStateIsModified_Then_StudentIsUpdated()
    {
        using var scoped = _fixture.ServiceProvider.CreateScope();
        var context = scoped.ServiceProvider.GetRequiredService<SchoolContext>();
        using var transaction = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var id = 2;

        var studentToUpdate = await context.Students.FindAsync([id], cancellationToken: TestContext.Current.CancellationToken);

        if (studentToUpdate is null)
        {
            Assert.Fail("return NotFound()");
        }

        EntityState? stateBeforeSave = null;

        if (await TryUpdateModelAsync<Student>(
            studentToUpdate,
            "student",
            s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
        {
            stateBeforeSave = context.Entry(studentToUpdate).State;
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        Assert.Equal(EntityState.Modified, stateBeforeSave);

        context.ChangeTracker.Clear();
        var student = await context.Students.FindAsync([id], cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(student);
        Assert.Equal("Joe", student.FirstMidName);
        Assert.Equal("Smith", student.LastName);
    }

    [Fact]
    public async Task When_EntityStateIsDeleted_Then_StudentIsDeleted()
    {
        using var scoped = _fixture.ServiceProvider.CreateScope();
        var context = scoped.ServiceProvider.GetRequiredService<SchoolContext>();
        using var transaction = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var id = 2;

        var studentToRemove = await context.Students.FindAsync([id], TestContext.Current.CancellationToken);

        if (studentToRemove is null)
        {
            Assert.Fail("return NotFound()");
        }

        EntityState? stateBeforeSave = null;

        try
        {
            context.Students.Remove(studentToRemove);

            stateBeforeSave = context.Entry(studentToRemove).State;
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }
        catch (DbUpdateException ex)
        {
            Assert.Fail(ex.Message);
        }

        Assert.Equal(EntityState.Deleted, stateBeforeSave);

        context.ChangeTracker.Clear();
        var student = await context.Students.FindAsync([id], cancellationToken: TestContext.Current.CancellationToken);

        Assert.Null(student);
    }


#pragma warning disable IDE0060 // Remove unused parameter

    // see: https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.tryupdatemodelasync?view=aspnetcore-10.0
    private static Task<bool> TryUpdateModelAsync<TModel>(TModel model,
        string prefix,
        params Expression<Func<TModel, object?>>[] valueProvider)
        where TModel : class
    {
        // Using TryUpdateModel to update fields with posted values 
        // is a security best practice because it prevents overposting.

        // Here is Fake.

        if (model is Student student)
        {
            student.EnrollmentDate = DateTime.Now;
            student.FirstMidName = "Joe";
            student.LastName = "Smith";
        }

        return Task.FromResult(true);
    }

#pragma warning restore IDE0060 // Remove unused parameter


}
