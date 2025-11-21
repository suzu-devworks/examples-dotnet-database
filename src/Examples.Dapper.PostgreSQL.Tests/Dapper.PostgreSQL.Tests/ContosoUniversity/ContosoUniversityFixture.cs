using ContosoUniversity.Abstraction;
using Examples.Dapper.PostgreSQL.ContosoUniversity;
using Examples.Dapper.PostgreSQL.ContosoUniversity.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Examples.Dapper.PostgreSQL.Tests.ContosoUniversity;

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

        ConfigurationServices(services);

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

    private static void ConfigurationServices(IServiceCollection services)
    {
        if (!DatabaseEnvironment.IsAvailable)
        {
            return;
        }

        var provider = services.BuildServiceProvider();
        var configuration = provider.GetRequiredService<IConfiguration>();
        var logger = provider.GetRequiredService<ILogger<ContosoUniversityFixture>>();

        var connectionString = configuration.GetConnectionString("ContosoUniversity")
            ?? throw new InvalidOperationException("ConnectionStrings:ContosoUniversity is required.");

        services.AddDbConnectionFactory(builder => builder.UseNpgsql(connectionString));

#if USE_FACTORY
        services.AddDbConnectionFactory(builder => builder.UseNpgsql(connectionString));

        services.AddScoped<IStudentRepository, StudentFactoryRepository>();
        logger.LogInformation("Use Factory.");
#else
        services.AddKeyedDbDataSource(
            DataSourceKeys.ContosoUniversity,
            builder => builder.UseNpgsql(connectionString));

        services.AddScoped<IStudentRepository, StudentRepository>();
        logger.LogInformation("Use DataSource.");
#endif

    }

    private static void InitializeDatabase()
    {
        if (!DatabaseEnvironment.IsAvailable)
        {
            return;
        }

        // TODO migration.
    }
}
