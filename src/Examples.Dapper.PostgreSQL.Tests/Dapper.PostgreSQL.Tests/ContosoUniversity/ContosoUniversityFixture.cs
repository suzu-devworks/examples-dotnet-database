using ContosoUniversity.Abstraction;
using Examples.Configuration;
using Examples.Dapper.PostgreSQL.ContosoUniversity.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Examples.Dapper.PostgreSQL.Tests.ContosoUniversity;

public class ContosoUniversityFixture : IDisposable
{
    public static bool Enabled => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("POSTGRES_SERVICE"));
    private static readonly Lock _lock = new();
    private static bool _databaseInitialized;

    private readonly ServiceProvider _serviceProvider;
    private Action<string>? _logging;

    public ContosoUniversityFixture()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddUserSecrets<ContosoUniversityFixture>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection();
        services.AddLoggingForFixtures(_logging);

        ConfigurationServices(services, configuration);

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

    private static void ConfigurationServices(IServiceCollection services, IConfiguration configuration)
    {
        if (!Enabled)
        {
            return;
        }

        var provider = services.BuildServiceProvider();
        var logger = provider.GetRequiredService<ILogger<ContosoUniversityFixture>>();

        var connectionString = configuration.GetRequiredConnectionString("ContosoUniversity");
        services.AddDbConnectionFactory(builder => builder.UseNpgsql(connectionString));
        services.AddKeyedDbDataSource(
            DataSourceKeys.ContosoUniversity,
            builder => builder.UseNpgsql(connectionString));

#if USE_FACTORY 
        services.AddScoped<IStudentRepository, StudentFactoryRepository>();
        logger.LogInformation("Use Factory.");
#else
        services.AddScoped<IStudentRepository, StudentRepository>();
        logger.LogInformation("Use DataSource.");
#endif
    }

    private static void InitializeDatabase()
    {
        if (!Enabled)
        {
            return;
        }

        // TODO migration.
    }
}
