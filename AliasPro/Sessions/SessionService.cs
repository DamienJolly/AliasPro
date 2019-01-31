using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Sessions
{
    using Network;

    internal class SessionService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<SessionRepository>();
            collection.AddSingleton<ISessionController, SessionController>();
        }
    }
}
