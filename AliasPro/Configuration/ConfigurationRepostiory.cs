using AliasPro.API.Configuration.Models;
using AliasPro.Configuration.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AliasPro.Configuration
{
    internal class ConfigurationRepostiory
    {
        private readonly ILogger<ConfigurationRepostiory> _logger;
        public IConfigurationData ConfigurationData;

        public ConfigurationRepostiory(ILogger<ConfigurationRepostiory> logger)
        {
            _logger = logger;
            ConfigurationData = ReadConfigurationFile();
        }

        private IConfigurationData ReadConfigurationFile()
        {
            IConfigurationData data = null;

            if (!File.Exists(GetFileFromDictionary(@"configuration.alias")))
            {
                _logger.LogCritical("Configuration File not found.");
                _logger.LogInformation("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            IDictionary<string, string> variables = new Dictionary<string, string>();
            foreach (string line in File.ReadAllLines(GetFileFromDictionary(@"configuration.alias")))
            {
                if (!line.StartsWith("#") && line.Contains("=") && line.Split('=').Length == 2 && 
                    !variables.ContainsKey(line.Split('=')[0]))
                {
                    variables.Add(line.Split('=')[0], line.Split('=')[1]);
                }
            }
            
            try
            {
                data = new ConfigurationData(variables);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Configuration File was missing some information.\n\n" + ex);
                _logger.LogInformation("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            return data;
        }

        //util
        private static string GetFileFromDictionary(string path)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
        }
    }
}
