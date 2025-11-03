using Examples.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Examples.EntityFrameworkCore.InMemory.Tests;

public static class LoggingBuilderExtensions
{
    public static IServiceCollection AddLoggingForFixtures(this IServiceCollection services, Action<string>? logging)
    {
        services.AddLogging(builder
            => builder.SetMinimumLevel(LogLevel.Information)
                .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Information)
                .AddFilter("Examples", LogLevel.Trace)
                .AddFilter("ContosoUniversity", LogLevel.Trace)
                .AddDelegateLogger(x =>
                {
                    var message = $"[{DateTime.Now:HH:mm:ss.fff}][{x.LogLevel}]: {x.Message}";
                    logging?.Invoke(message);
                    System.Diagnostics.Debug.WriteLine(message); // for vscode
                }));

        return services;
    }
}
