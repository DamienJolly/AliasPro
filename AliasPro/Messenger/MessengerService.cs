using AliasPro.API.Messenger;
using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.Messenger.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Messenger
{
    internal class MessengerService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<MessengerDao>();
            collection.AddSingleton<MessengerRepository>();
            collection.AddSingleton<IMessengerController, MessengerController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {
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
            collection.AddSingleton<IAsyncPacket, ChangeRelationEvent>();
        }
    }
}
