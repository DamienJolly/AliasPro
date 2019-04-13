using AliasPro.API.Network;
using AliasPro.API.Sessions;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Sessions
{
    internal class SessionService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<SessionRepository>();
            collection.AddSingleton<ISessionController, SessionController>();
        }
    }
}
