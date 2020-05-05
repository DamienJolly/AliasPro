using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Game.Badges
{
    internal class BadgeService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<BadgeDao>();
            collection.AddSingleton<BadgeController>();
        }
    }
}
