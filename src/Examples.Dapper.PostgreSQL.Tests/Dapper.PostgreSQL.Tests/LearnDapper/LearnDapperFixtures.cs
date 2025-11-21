using Examples.Dapper.PostgreSQL.ProductCatalogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL.Tests.LearnDapper;

public class LearnDapperFixtures : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private Action<string>? _logging;

    public LearnDapperFixtures()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddUserSecrets<LearnDapperFixtures>(optional: true)
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

    public LearnDapperFixtures UseLogger(Action<string> loggerAction)
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

        var connectionString = configuration.GetConnectionString("ProductCatalogs")
            ?? throw new InvalidOperationException("ConnectionStrings:ProductCatalogs is required.");

        services.AddKeyedDbDataSource(
            DataSourceKeys.ProductCatalogs,
            builder => builder.UseNpgsql(connectionString));

    }

}
