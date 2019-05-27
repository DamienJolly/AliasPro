using AliasPro.API.Groups;
using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.Groups.Packets.Events;
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
			collection.AddSingleton<IAsyncPacket, RequestGroupInfoEvent>();
			collection.AddSingleton<IAsyncPacket, RequestGroupBuyRoomsEvent>();
			collection.AddSingleton<IAsyncPacket, RequestGroupPartsEvent>();
			collection.AddSingleton<IAsyncPacket, RequestGroupBuyEvent>();
			collection.AddSingleton<IAsyncPacket, RequestGroupMembersEvent>();
			collection.AddSingleton<IAsyncPacket, RequestGroupManageEvent>();
			collection.AddSingleton<IAsyncPacket, GroupSetAdminEvent>();
			collection.AddSingleton<IAsyncPacket, GroupRemoveAdminEvent>();
			collection.AddSingleton<IAsyncPacket, RequestGroupJoinEvent>();
			collection.AddSingleton<IAsyncPacket, GroupRemoveMemberEvent>();
			collection.AddSingleton<IAsyncPacket, GroupAcceptMembershipEvent>();
		}
	}
}
