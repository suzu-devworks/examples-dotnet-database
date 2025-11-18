using Microsoft.Extensions.Logging;

namespace Examples.Logging;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddDelegateLogger(this ILoggingBuilder builder,
        Action<DelegateLoggerProvider.LogRecord> action)
    {
        builder.AddProvider(new DelegateLoggerProvider(action));

        return builder;
    }
}
