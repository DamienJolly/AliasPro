using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Components;
using AliasPro.Sessions;
using System.Collections.Generic;

namespace AliasPro.Messenger.Packets.Events
{
    public class RequestInitFriendsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestInitFriendsMessageEvent;

        private readonly IMessengerController _messengerController;

        public RequestInitFriendsEvent(IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: add to cofig
            int maxFriends = 1000;

            if (session.Player.Messenger == null)
            {
                session.Player.Messenger = new MessengerComponent(
                    await _messengerController.GetPlayerFriendsAsync(session.Player.Id),
                    await _messengerController.GetPlayerRequestsAsync(session.Player.Id));

                await _messengerController.UpdateStatusAsync(session.Player, session.Player.Messenger.Friends);
            }

            await session.SendPacketAsync(new MessengerInitComposer(maxFriends));
            await session.SendPacketAsync(new FriendsComposer(session.Player.Messenger.Friends));
            
            ICollection<IMessengerMessage> messages = 
                await _messengerController.GetOfflineMessagesAsync(session.Player.Id);

            foreach (IMessengerMessage message in messages)
                await session.SendPacketAsync(new FriendChatComposer(message));
        }
    }
}
