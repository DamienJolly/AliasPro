using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.Messenger.Packets.Outgoing;
using AliasPro.Network.Events;
using AliasPro.Network.Protocol;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Incoming
{
    using Network.Events.Headers;

    public class ChangeRelationEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ChangeRelationMessageEvent;

        private readonly IMessengerController _messengerController;

        public ChangeRelationEvent(IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async Task HandleAsync(
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
