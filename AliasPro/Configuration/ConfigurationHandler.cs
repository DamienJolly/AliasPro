using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace AliasPro.Configuration
{
	public class ConfigurationHandler
	{
        private static readonly string fileName = "config.xml";

        private readonly ILogger<ConfigurationHandler> logger;

        private readonly Dictionary<string, string> configValues;

        public ConfigurationHandler(
            ILogger<ConfigurationHandler> logger)
        {
            this.logger = logger;

            this.configValues = new Dictionary<string, string>();
        }

        public void ReadConfigurationFile()
        {
            if (!File.Exists(fileName))
            {
                WriteConfig();
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);

            SetConfig(xmlDoc, "mysql/hostname");
            SetConfig(xmlDoc, "mysql/username");
            SetConfig(xmlDoc, "mysql/password");
            SetConfig(xmlDoc, "mysql/database");
            SetConfig(xmlDoc, "mysql/port");
            SetConfig(xmlDoc, "mysql/min_connections", "mincon");
            SetConfig(xmlDoc, "mysql/max_connections", "maxcon");
            SetConfig(xmlDoc, "server/ip");
            SetConfig(xmlDoc, "server/port");
        }

        public string GetString(string category, string item) => 
            configValues.GetValueOrDefault(string.Concat(category, "/", item));

        public int GetInt(string category, string item)
        {
            int.TryParse(configValues.GetValueOrDefault(string.Concat(category, "/", item)), out int number);
            return number;
        }

        private void SetConfig(XmlDocument xmlDoc, string xmlPath, string configKey = null)
        {
            try
            {
                configValues[configKey != null ? xmlPath.Split('/')[0] + "/" + configKey : xmlPath] = xmlDoc.SelectSingleNode("//configuration/" + xmlPath).InnerText;
            }
            catch
            {
                logger.LogWarning("Failed to load config value: " + xmlPath.Split('/')[0] + "->" + xmlPath.Split('/')[1]);
                throw;
            }
        }

        private void WriteConfig()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("   ");
            settings.OmitXmlDeclaration = true;

            XmlWriter xmlWriter = XmlWriter.Create(fileName, settings);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("configuration");
            xmlWriter.WriteStartElement("mysql");

            xmlWriter.WriteStartElement("hostname");
            xmlWriter.WriteString("localhost");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("username");
            xmlWriter.WriteString("root");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("password");
            xmlWriter.WriteString("");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("database");
            xmlWriter.WriteString("alias");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("port");
            xmlWriter.WriteString("3306");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("min_connections");
            xmlWriter.WriteString("5");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("max_connections");
            xmlWriter.WriteString("10");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("server");

            xmlWriter.WriteStartElement("ip");
            xmlWriter.WriteString("127.0.0.1");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("port");
            xmlWriter.WriteString("30000");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }
    }
}
