using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace AliasPro.Configuration.Models
{
    internal class ConfigurationData : IConfigurationData
    {
        public ConfigurationData(IDictionary<string, string> configData)
        {
            ServerIPAddress = configData["tcp.host"];
            ServerPort = int.Parse(configData["tcp.port"]);
            ServerMaxConnections = int.Parse(configData["tcp.conlimit"]);
            ServerMaxConnectionsPerIP = int.Parse(configData["tcp.conperip"]);
            MySQLHostName = configData["mysql.hostname"];
            MySQLPort = uint.Parse(configData["mysql.port"]);
            MySQLUsername = configData["mysql.username"];
            MySQLPassword = configData["mysql.password"];
            MySQLDatabase = configData["mysql.database"];
            MySQLMinimumPoolSize = uint.Parse(configData["mysql.minsize"]);
            MySQLMaximumPoolSize = uint.Parse(configData["mysql.maxsize"]);
        }

        public string ConnectionString
        {
            get
            {
                MySqlConnectionStringBuilder ConnString = new MySqlConnectionStringBuilder
                {
                    Server = MySQLHostName,
                    Port = MySQLPort,
                    UserID = MySQLUsername,
                    Password = MySQLPassword,
                    Database = MySQLDatabase,
                    MinimumPoolSize = MySQLMinimumPoolSize,
                    MaximumPoolSize = MySQLMaximumPoolSize,
                    Pooling = true,
                    SslMode = MySqlSslMode.None
                };
                return ConnString.ToString();
            }
        }

        public string ServerIPAddress { get; }
        public int ServerPort { get; }
        public int ServerMaxConnections { get; }
        public int ServerMaxConnectionsPerIP { get; }
        public string MySQLHostName { get; }
        public uint MySQLPort { get; }
        public string MySQLUsername { get; }
        public string MySQLPassword { get; }
        public string MySQLDatabase { get; }
        public uint MySQLMinimumPoolSize { get; }
        public uint MySQLMaximumPoolSize { get; }
    }

    public interface IConfigurationData
    {
        string ConnectionString { get; }

        string ServerIPAddress { get; }
        int ServerPort { get; }
        int ServerMaxConnections { get; }
        int ServerMaxConnectionsPerIP { get; }
        string MySQLHostName { get; }
        uint MySQLPort { get; }
        string MySQLUsername { get; }
        string MySQLPassword { get; }
        string MySQLDatabase { get; }
        uint MySQLMinimumPoolSize { get; }
        uint MySQLMaximumPoolSize { get; }
    }
}
