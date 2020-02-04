using AliasPro.API.Messenger;
using AliasPro.Communication.Messages;
using AliasPro.Messenger.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Messenger
{
    internal class MessengerService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<MessengerDao>();
            collection.AddSingleton<MessengerRepository>();
            collection.AddSingleton<IMessengerController, MessengerController>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IMessageEvent, RequestInitFriendsEvent>();
            collection.AddSingleton<IMessageEvent, FriendRequestEvent>();
            collection.AddSingleton<IMessageEvent, RequestFriendsEvent>();
            collection.AddSingleton<IMessageEvent, RequestFriendRequestsEvent>();
            collection.AddSingleton<IMessageEvent, DeclineFriendRequestEvent>();
            collection.AddSingleton<IMessageEvent, AcceptFriendRequestEvent>();
            collection.AddSingleton<IMessageEvent, RemoveFriendEvent>();
            collection.AddSingleton<IMessageEvent, SearchUserEvent>();
            collection.AddSingleton<IMessageEvent, FriendPrivateMessageEvent>();
            collection.AddSingleton<IMessageEvent, RoomInviteEvent>();
            collection.AddSingleton<IMessageEvent, ChangeRelationEvent>();
            collection.AddSingleton<IMessageEvent, StalkFriendEvent>();
        }
    }
}
