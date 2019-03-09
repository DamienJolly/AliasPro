using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Player
{
    using Network;
    using Network.Events;
    using Packets.Incoming;

    internal class PlayerService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<PlayerDao>();
            collection.AddSingleton<PlayerRepostiory>();
            collection.AddSingleton<IPlayerController, PlayerController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IAsyncPacket, UniqueIdEvent>();
            collection.AddSingleton<IAsyncPacket, SecureLoginEvent>();
            collection.AddSingleton<IAsyncPacket, RequestUserDataEvent>();
            collection.AddSingleton<IAsyncPacket, RequestFurniInventoryEvent>();
            collection.AddSingleton<IAsyncPacket, RequestBadgeInventoryEvent>();
            collection.AddSingleton<IAsyncPacket, RequestWearingBadgesEvent>();
            collection.AddSingleton<IAsyncPacket, UserWearBadgeEvent>();
            collection.AddSingleton<IAsyncPacket, RequestUserCreditsEvent>();
            collection.AddSingleton<IAsyncPacket, RequestUserProfileEvent>();
            collection.AddSingleton<IAsyncPacket, RequestInitFriendsEvent>();
            collection.AddSingleton<IAsyncPacket, FriendRequestEvent>();
            collection.AddSingleton<IAsyncPacket, RequestFriendsEvent>();
            collection.AddSingleton<IAsyncPacket, RequestFriendRequestsEvent>();
            collection.AddSingleton<IAsyncPacket, DeclineFriendRequestEvent>();
            collection.AddSingleton<IAsyncPacket, AcceptFriendRequestEvent>();
            collection.AddSingleton<IAsyncPacket, RemoveFriendEvent>();
            collection.AddSingleton<IAsyncPacket, SearchUserEvent>();
            collection.AddSingleton<IAsyncPacket, FriendPrivateMessageEvent>();
            collection.AddSingleton<IAsyncPacket, RoomInviteEvent>();
            collection.AddSingleton<IAsyncPacket, RequestProfileFriendsEvent>();
            collection.AddSingleton<IAsyncPacket, ChangeRelationEvent>();
        }
    }
}
