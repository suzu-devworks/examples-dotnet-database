using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL.Tests.Dialect;

public class DialectFixtures : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private Action<string>? _logging;

    public DialectFixtures()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddUserSecrets<DialectFixtures>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection();
        services.AddLoggingForFixtures(_logging);

        ConfigurationServices(services, configuration);

        _serviceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
        GC.SuppressFinalize(this);
    }

    public IServiceProvider ServiceProvider => _serviceProvider;

    public DialectFixtures UseLogger(Action<string> loggerAction)
    {
        _logging = loggerAction;
        return this;
    }

    private static void ConfigurationServices(ServiceCollection services, IConfiguration configuration)
    {
        if (!DatabaseEnvironment.IsAvailable)
        {
            return;
        }

        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("ConnectionStrings:Default is required.");

        services.AddDbDataSource(builder => builder.UseNpgsql(connectionString));
    }

}
