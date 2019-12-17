using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace AliasPro.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        public void Dispose() { }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomConsoleLogger(categoryName);
        }

        public class CustomConsoleLogger : ILogger
        {
            private readonly string _categoryName;

            public CustomConsoleLogger(string categoryName)
            {
                _categoryName = categoryName;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                string level = $"{logLevel}";

                switch (logLevel)
                {
                    default:
                    case LogLevel.Information:
                        {
                            if (_categoryName == "AliasPro.Network.Events.EventProvider")
                            {
                                if (!Debugger.IsAttached)
                                    return;

                                level = "Packet";
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                            break;
                        }
                    case LogLevel.Warning:
                    case LogLevel.Error:
                    case LogLevel.Critical:
                        {
                            if (_categoryName == "AliasPro.Network.Events.EventProvider")
                            {
                                if (!Debugger.IsAttached)
                                    return;

                                level = "Packet";
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            break;
                        }
                }

                Console.WriteLine($"[Alias] [{level}] : {formatter(state, exception)}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}
