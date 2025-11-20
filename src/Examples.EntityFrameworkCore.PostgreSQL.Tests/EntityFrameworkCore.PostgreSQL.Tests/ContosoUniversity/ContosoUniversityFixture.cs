using ContosoUniversity.Abstraction;
using ContosoUniversity.Data;
using ContosoUniversity.Repositories;
using Examples.EntityFrameworkCore.PostgreSQL.ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.EntityFrameworkCore.PostgreSQL.Tests.ContosoUniversity;

public class ContosoUniversityFixture : IDisposable
{
    private static readonly Lock _lock = new();
    private static bool _databaseInitialized;

    private readonly ServiceProvider _serviceProvider;
    private Action<string>? _logging;

    public ContosoUniversityFixture()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddUserSecrets<ContosoUniversityFixture>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLoggingForFixtures(_logging);

        ConfigureServices(services);

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

    private static void ConfigureServices(ServiceCollection services)
    {
        if (!DatabaseEnvironment.IsAvailable)
        {
            return;
        }

        var provider = services.BuildServiceProvider();
        var configuration = provider.GetRequiredService<IConfiguration>();

        var connectionString = configuration.GetConnectionString("ContosoUniversity")
            ?? throw new InvalidOperationException("ConnectionStrings:ContosoUniversity is required.");

        services.AddDbContext<SchoolContext, NpgsqlSchoolContext>(builder
            => builder.UseNpgsql(connectionString)
        );

        services.AddScoped<IStudentRepository, StudentRepository>();
    }

    private void InitializeDatabase()
    {
        if (!DatabaseEnvironment.IsAvailable)
        {
            return;
        }

        var context = ServiceProvider.GetRequiredService<SchoolContext>();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        DbInitializer.Initialize(context);

        context.SaveChanges();
    }

}
