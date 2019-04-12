using AliasPro.API.Configuration.Models;

namespace AliasPro.API.Configuration
{
    public interface IConfigurationController
    {
        IConfigurationData ConfigurationData { get; }
    }
}
