using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Players.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Events
{
    public class RequestInitFriendsEvent : IMessageEvent
    {
        public short Header => Incoming.RequestInitFriendsMessageEvent;

        private readonly IMessengerController _messengerController;

        public RequestInitFriendsEvent(IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
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

            foreach (IMessengerMessage msg in messages)
                await session.SendPacketAsync(new FriendChatComposer(msg));
        }
    }
}
