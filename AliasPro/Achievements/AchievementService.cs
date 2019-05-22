using AliasPro.Achievements.Packets.Events;
using AliasPro.API.Achievements;
using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Achievements
{
    internal class AchievementService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<AchievementDao>();
            collection.AddSingleton<IAchievementController, AchievementController>();

			AddPackets(collection);
		}

		private static void AddPackets(IServiceCollection collection)
		{
			collection.AddSingleton<IAsyncPacket, RequestAchievementsEvent>();
		}
	}
}
