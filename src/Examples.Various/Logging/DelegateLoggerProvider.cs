using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Examples.Logging;

/// <summary>
/// Implementing <see cref="ILoggerProvider" /> to delegate log output.
/// </summary>
/// <examples>
/// <code>
/// dotnet test -l "console;verbosity=detailed"
/// </code>
/// </examples>
[ProviderAlias("Delegate")]
public class DelegateLoggerProvider(
    Action<DelegateLoggerProvider.LogRecord> action
    ) : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, DelegateLogger> _loggers = [];

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new DelegateLogger(name, action));
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _loggers.Clear();
        GC.SuppressFinalize(this);
    }

    private class DelegateLogger(
        string categoryName,
        Action<LogRecord> action
        ) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            ArgumentNullException.ThrowIfNull(formatter);

            string formatted = formatter(state, exception);

            if (string.IsNullOrEmpty(formatted) && exception == null)
            {
                return;
            }

            string message;
            if (!string.IsNullOrEmpty(formatted) && exception is not null)
            {
                message = $"{formatted}{Environment.NewLine}{Environment.NewLine}{exception}";
            }
            else if (!string.IsNullOrEmpty(formatted))
            {
                message = $"{formatted}";
            }
            else
            {
                message = $"{exception}";
            }

            var record = new LogRecord(categoryName, logLevel, message, exception);
            action(record);
        }
    }

    public record LogRecord(
        string CategoryName,
        LogLevel LogLevel,
        string Message,
        Exception? Exception
        );

    private class NullScope : IDisposable
    {
        public static NullScope Instance = new();
        private NullScope() { }
        public void Dispose() { }
    }

}
