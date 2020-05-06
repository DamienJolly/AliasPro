using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Game.Achievements
{
    internal class AchievementService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<AchievementDao>();
            collection.AddSingleton<AchievementController>();
        }
    }
}
