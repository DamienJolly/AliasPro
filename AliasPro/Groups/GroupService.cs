using AliasPro.API.Groups;
using AliasPro.Communication.Messages;
using AliasPro.Groups.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Groups
{
    internal class GroupService : IService
	{
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<GroupDao>();
            collection.AddSingleton<IGroupController, GroupController>();

			RegisterPackets(collection);
		}

		private static void RegisterPackets(IServiceCollection collection)
		{
			collection.AddSingleton<IMessageEvent, RequestGroupInfoEvent>();
			collection.AddSingleton<IMessageEvent, RequestGroupBuyRoomsEvent>();
			collection.AddSingleton<IMessageEvent, RequestGroupPartsEvent>();
			collection.AddSingleton<IMessageEvent, RequestGroupBuyEvent>();
			collection.AddSingleton<IMessageEvent, RequestGroupMembersEvent>();
			collection.AddSingleton<IMessageEvent, GroupSetAdminEvent>();
			collection.AddSingleton<IMessageEvent, GroupRemoveAdminEvent>();
			collection.AddSingleton<IMessageEvent, RequestGroupJoinEvent>();
			collection.AddSingleton<IMessageEvent, GroupRemoveMemberEvent>();
			collection.AddSingleton<IMessageEvent, GroupAcceptMembershipEvent>();
			collection.AddSingleton<IMessageEvent, GroupDeclineMembershipEvent>();
			collection.AddSingleton<IMessageEvent, RequestGroupManageEvent>();
			collection.AddSingleton<IMessageEvent, GroupChangeBadgeEvent>();
			collection.AddSingleton<IMessageEvent, GroupChangeColorsEvent>();
			collection.AddSingleton<IMessageEvent, GroupChangeNameDescEvent>();
			collection.AddSingleton<IMessageEvent, GroupChangeSettingsEvent>();
			collection.AddSingleton<IMessageEvent, GroupDeleteEvent>();
			collection.AddSingleton<IMessageEvent, GroupSetFavoriteEvent>();
			collection.AddSingleton<IMessageEvent, GroupRemoveFavoriteEvent>();
		}
	}
}
