using AliasPro.Configuration.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AliasPro.Configuration
{
    internal class ConfigurationRepostiory
    {
        public IConfigurationData ConfigurationData;

        public ConfigurationRepostiory()
        {
            ConfigurationData = ReadConfigurationFile();
        }

        private IConfigurationData ReadConfigurationFile()
        {
            IConfigurationData data = null;

            if (!File.Exists(GetFileFromDictionary(@"configuration.alias")))
            {
                //todo: use logger
                Console.WriteLine("Configuration File not found.");
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
                Console.WriteLine(ex);
                Console.WriteLine("Configuration File was missing some information.");
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
