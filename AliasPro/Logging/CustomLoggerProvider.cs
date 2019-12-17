using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace AliasPro.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        public void Dispose() { }

        public ILogger CreateLogger(string categoryName)
        {
            CreateFile(@"exceptions.alias");
            return new CustomConsoleLogger(categoryName);
        }

        private void CreateFile(string fileName)
        {
            var myFile = File.Create(fileName);
            myFile.Close();
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
                                level = "Packet";
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
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

                Console.WriteLine($"[Alias] [{level}] : {state}");
                Console.ForegroundColor = ConsoleColor.Gray;

                if (logLevel == LogLevel.Error && exception != null)
                {
                    string currentText = File.ReadAllText(@"exceptions.alias");
                    currentText += "\n\n";
                    currentText += "Date: " + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                    currentText += "\nEmulator Information: " + $"{state}";
                    currentText += "\nInformation for developer: " + exception.ToString();
                    File.WriteAllText(@"exceptions.alias", currentText);
                }

                if (logLevel == LogLevel.Critical)
                {
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
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
