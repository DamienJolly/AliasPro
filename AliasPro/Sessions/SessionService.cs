using AliasPro.API.Sessions;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Sessions
{
    internal class SessionService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<SessionRepository>();
            collection.AddSingleton<ISessionController, SessionController>();
        }
    }
}
