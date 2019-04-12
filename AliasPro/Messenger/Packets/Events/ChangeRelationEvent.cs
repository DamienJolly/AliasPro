using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Messenger.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Sessions;

namespace AliasPro.Messenger.Packets.Events
{
    public class ChangeRelationEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ChangeRelationMessageEvent;

        private readonly IMessengerController _messengerController;

        public ChangeRelationEvent(IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int playerId = clientPacket.ReadInt();
            if (!session.Player.Messenger.TryGetFriend((uint)playerId, out IMessengerFriend friend))
                return;

            int type = clientPacket.ReadInt();
            if (type < 0 || type > 3)
                return;

            friend.Relation = type;
            await _messengerController.UpdateRelationAsync(session.Player.Id, friend);
            await session.SendPacketAsync(new UpdateFriendComposer(friend));
        }
    }
}
