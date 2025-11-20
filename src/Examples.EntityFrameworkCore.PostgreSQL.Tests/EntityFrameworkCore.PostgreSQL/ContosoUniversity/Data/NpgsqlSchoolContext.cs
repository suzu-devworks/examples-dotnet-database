using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Examples.EntityFrameworkCore.Data;
using Microsoft.EntityFrameworkCore;

namespace Examples.EntityFrameworkCore.PostgreSQL.ContosoUniversity.Data;

public class NpgsqlSchoolContext(DbContextOptions<NpgsqlSchoolContext> options)
    : SchoolContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>()
            .ToTable("students", "user")
            .Property(x => x.EnrollmentDate)
            .HasConversion<UniversalDateTimeConvertor>();

        modelBuilder.Entity<Enrollment>().ToTable("enrollments", "user");

        modelBuilder.Entity<Course>().ToTable("courses", "user");
    }

}
