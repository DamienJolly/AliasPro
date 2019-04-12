namespace AliasPro.API.Configuration.Models
{
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
