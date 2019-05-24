using AliasPro.API.Groups;
using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Groups
{
    internal class GroupService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<GroupDao>();
            collection.AddSingleton<IGroupController, GroupController>();

			AddPackets(collection);
		}

		private static void AddPackets(IServiceCollection collection)
		{

		}
	}
}
