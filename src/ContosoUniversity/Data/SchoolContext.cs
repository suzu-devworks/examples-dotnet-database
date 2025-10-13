using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Data;

public class SchoolContext(DbContextOptions<SchoolContext> options)
    : DbContext(options)
{
    public override void Dispose()
    {
        System.Diagnostics.Debug.WriteLine("Dispose called.");
        base.Dispose();
        GC.SuppressFinalize(this);
    }

    public DbSet<Student> Students { get; set; } = default!;
    public DbSet<Enrollment> Enrollments { get; set; } = default!;
    public DbSet<Course> Courses { get; set; } = default!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>().ToTable("Students", "user");
        modelBuilder.Entity<Enrollment>().ToTable("Enrollments", "user");
        modelBuilder.Entity<Course>().ToTable("Courses", "user");
    }

}
