using ContosoUniversity.Abstraction;
using Examples.Dapper.SqlServer.ContosoUniversity;
using Examples.Dapper.SqlServer.ContosoUniversity.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.SqlServer.Tests.ContosoUniversity;

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

        var connectionString = configuration.GetConnectionString("ContosoUniversity")
            ?? throw new InvalidOperationException("ConnectionStrings:ContosoUniversity is required.");

        services.AddKeyedDbDataSource(
            DataSourceKeys.ContosoUniversity,
            builder => builder.UseSqlServer(connectionString));
        services.AddScoped<IStudentRepository, StudentRepository>();
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
