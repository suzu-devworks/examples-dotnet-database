using ContosoUniversity.Abstraction;
using ContosoUniversity.Data;
using ContosoUniversity.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.EntityFrameworkCore.InMemory.Tests.ContosoUniversity;

public class ContosoUniversityFixture : IDisposable
{
    private static readonly Lock _lock = new();

    private readonly ServiceProvider _serviceProvider;
    private Action<string>? _logging;

    public ContosoUniversityFixture()
    {
        var services = new ServiceCollection();
        services.AddLoggingForFixtures(_logging);

        services.AddTransient(service =>
        {
            // Since there are no transactions, the same database needs to be created every time.
            var options = new DbContextOptionsBuilder<SchoolContext>()
                .UseInMemoryDatabase(nameof(ContosoUniversityFixture))
                .ConfigureWarnings(o => o.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var context = new SchoolContext(options);

            lock (_lock)
            {
                InitializeDatabase(context);
            }

            return context;
        });

        // Since it's necessary to recreate the `DbContext` every time, it's a transient scope.
        services.AddTransient<IStudentRepository, StudentRepository>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
        GC.SuppressFinalize(this);
    }

    public IServiceProvider ServiceProvider => _serviceProvider;

    public ContosoUniversityFixture UseLogger(Action<string> loggerAction)
    {
        _logging = loggerAction;
        return this;
    }

    private static void InitializeDatabase(SchoolContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInitializer.Initialize(context);

        context.SaveChanges();
    }

}
