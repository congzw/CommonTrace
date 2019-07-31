using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CommonTrace.Jaeger
{
    public class MyLoggerFactory : ILoggerFactory
    {
        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger();
        }

        public void AddProvider(ILoggerProvider provider)
        {
        }
    }

    public class MyLogger : ILogger, IDisposable
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (exception != null)
            {
                Trace.WriteLine(exception.Message);
                return;
            }
            Trace.WriteLine(formatter(state, null));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
