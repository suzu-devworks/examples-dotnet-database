using ContosoUniversity.Abstraction;
using ContosoUniversity.Data;
using ContosoUniversity.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.EntityFrameworkCore.SQLite.Tests.ContosoUniversity;

public class ContosoUniversityFixture : IDisposable
{
    private static readonly Lock Lock = new();
    private static bool _databaseInitialized;

    private readonly ServiceProvider _serviceProvider;
    private Action<string>? _logging;

    public ContosoUniversityFixture()
    {
        Dictionary<string, string?> configValues = [];
        IConfigurationRoot configuration = new ConfigurationBuilder()
           .AddInMemoryCollection(configValues)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLoggingForFixtures(_logging);

        var connectionString = @"Data Source=Sharable:Mode=Memory;Cache=Shared";
        services.AddDbContext<SchoolContext>(builder
            => builder.UseSqlite(connectionString)
        );

        services.AddScoped<IStudentRepository, StudentRepository>();

        _serviceProvider = services.BuildServiceProvider();

        lock (Lock)
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
