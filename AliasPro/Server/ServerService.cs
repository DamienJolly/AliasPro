using AliasPro.API.Server;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Server
{
    internal class ServerService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<ServerDao>();
            collection.AddSingleton<IServerController, ServerController>();
        }
    }
}
