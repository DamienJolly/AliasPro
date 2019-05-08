using AliasPro.API.Network;
using AliasPro.API.Server;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Server
{
    internal class ServerService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<IServerController, ServerController>();
        }
    }
}
