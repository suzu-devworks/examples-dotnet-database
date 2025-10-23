using ContosoUniversity.Data;
using ContosoUniversity.Repositories;
using Examples.ContosoUniversity.PostgreSQL.Data;
using Examples.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Examples.ContosoUniversity.PostgreSQL.Tests;

public class ContosoUniversityFixture : IDisposable
{
    public static bool Enabled => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("POSTGRES_SERVICE"));
    private static readonly Lock _lock = new();
    private static bool _databaseInitialized;

    private readonly ServiceProvider _serviceProvider;
    private Action<string>? _logging;

    public ContosoUniversityFixture()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder
            // TODO SQL logging
            => builder.SetMinimumLevel(LogLevel.Information)
                .AddFilter("Npgsql", LogLevel.Debug)
                .AddFilter("Examples", LogLevel.Trace)
                .AddFilter("ContosoUniversity", LogLevel.Trace)
                .AddDelegateLogger(x => _logging?.Invoke($"[{DateTime.Now:HH:mm:ss.fff}] {x.LogLevel}: {x.Message}")));

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

    private static string GetConnectionString()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddUserSecrets<ContosoUniversityFixture>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("ContosoUniversity")
            ?? throw new InvalidOperationException("ConnectionStrings:ContosoUniversity is required.");

        return connectionString;
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        if (!Enabled)
        {
            return;
        }

        var connectionString = GetConnectionString();
        services.AddScoped<IDbConnectionProvider>(services => new NpgsqlConnectionProvider(connectionString));

        services.AddScoped<IStudentRepository, StudentRepository>();
    }

    private void InitializeDatabase()
    {
        if (!Enabled)
        {
            return;
        }

        // TODO migration.

        _ = ServiceProvider.GetRequiredService<IDbConnectionProvider>();

        // context.Database.EnsureDeleted();
        // context.Database.EnsureCreated();

        // DbInitializer.Initialize(context);

        // context.SaveChanges();
    }
}
