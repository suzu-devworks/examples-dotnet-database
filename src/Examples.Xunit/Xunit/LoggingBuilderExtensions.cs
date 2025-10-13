using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Examples.Xunit;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddXunit(this ILoggingBuilder builder, ITestOutputHelper helper)
    {
        builder.AddXunitDebug()
            .AddProvider(new XunitOutputLoggerProvider(helper));

        return builder;
    }

    public static ILoggingBuilder AddXunitDebug(this ILoggingBuilder builder)
    {
        builder.SetMinimumLevel(LogLevel.Trace);
        builder
            .AddFilter("Default", LogLevel.Information)
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("Microsoft.EntityFrameworkCore.Database", LogLevel.Information)
            .AddFilter("Examples", LogLevel.Trace);

        builder.AddDebug();

        return builder;
    }

}
