using ContosoUniversity.EntityFrameworkCore.Data;
using ContosoUniversity.EntityFrameworkCore.Repositories;
using ContosoUniversity.Repositories;
using Examples.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Examples.ContosoUniversity.SQLite.Tests;

public class ContosoUniversityFixture : IDisposable
{
    private static readonly Lock _lock = new();
    private static bool _databaseInitialized;

    private readonly ServiceProvider _serviceProvider;
    private Action<string>? _logging;

    public ContosoUniversityFixture()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder
            => builder.SetMinimumLevel(LogLevel.Warning)
                .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Information)
                .AddFilter("Examples", LogLevel.Trace)
                .AddFilter("ContosoUniversity", LogLevel.Trace)
                .AddDelegateLogger(x => _logging?.Invoke($"[{DateTime.Now:HH:mm:ss.fff}] {x.LogLevel}: {x.Message}")));

        var connectionString = @"Data Source=Sharable:Mode=Memory;Cache=Shared";
        services.AddDbContext<SchoolContext>(builder
            => builder.UseSqlite(connectionString)
        );

        services.AddScoped<IStudentRepository, StudentRepository>();

        _serviceProvider = services.BuildServiceProvider();

        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                InitializeDatabase();
                _databaseInitialized = true;
            }
        }
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

    private void InitializeDatabase()
    {
        var context = ServiceProvider.GetRequiredService<SchoolContext>();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInitializer.Initialize(context);

        context.SaveChanges();
    }

}
