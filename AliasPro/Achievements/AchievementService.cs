using AliasPro.Achievements.Packets.Events;
using AliasPro.API.Achievements;
using AliasPro.Communication.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Achievements
{
    internal class AchievementService : IService
	{
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<AchievementDao>();
            collection.AddSingleton<IAchievementController, AchievementController>();

			RegisterPackets(collection);
		}

		private static void RegisterPackets(IServiceCollection collection)
		{
			collection.AddSingleton<IMessageEvent, RequestAchievementsEvent>();
		}
	}
}
