using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Examples.EntityFrameworkCore.Data;
using Microsoft.EntityFrameworkCore;

namespace Examples.EntityFrameworkCore.PostgreSQL.Tests.ContosoUniversity.Data;

public class PgsqlSchoolContext(DbContextOptions<PgsqlSchoolContext> options)
    : SchoolContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>()
            .Property(x => x.EnrollmentDate)
            .HasConversion<UniversalDateTimeConvertor>();
    }

}
