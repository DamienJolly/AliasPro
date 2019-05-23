using AliasPro.API.Badge;
using AliasPro.API.Network;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Badges
{
    internal class BadgeService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<BadgeDao>();
            collection.AddSingleton<IBadgeController, BadgeController>();

			AddPackets(collection);
		}

		private static void AddPackets(IServiceCollection collection)
		{

		}
	}
}
