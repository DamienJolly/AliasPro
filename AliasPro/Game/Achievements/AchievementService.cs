using AliasPro.Communication.Messages;
using AliasPro.Game.Achievements.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Game.Achievements
{
    public class AchievementService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<AchievementDao>();
            collection.AddSingleton<AchievementController>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IMessageEvent, RequestAchievementsEvent>();
        }
    }
}
