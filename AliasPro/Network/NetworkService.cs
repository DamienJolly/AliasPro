using AliasPro.API.Network;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Network
{
    internal class NetworkService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<NetworkInitializer>();
            collection.AddSingleton<INetworkListener, NetworkListener>();
        }
    }
}
