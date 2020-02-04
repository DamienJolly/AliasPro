using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Game.Sessions
{
    public class SessionService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<SessionController>();
            collection.AddSingleton<SessionFactory>();
        }
    }
}
