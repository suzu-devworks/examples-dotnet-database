using Microsoft.Extensions.Configuration;

namespace Examples.Configuration;

public static class ConfigurationExtensions
{
    public static string GetRequiredConnectionString(this IConfiguration configuration, string name)
    {
        return configuration.GetConnectionString(name)
            ?? throw new InvalidOperationException($"Configuration: ConnectionStrings:{name} is required.");
    }

}
