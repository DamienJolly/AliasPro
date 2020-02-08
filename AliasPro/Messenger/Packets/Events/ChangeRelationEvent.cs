using AliasPro.API.Messenger;
using AliasPro.API.Messenger.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Messenger.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Events
{
    public class ChangeRelationEvent : IMessageEvent
    {
        public short Header => Incoming.ChangeRelationMessageEvent;

        private readonly IMessengerController _messengerController;

        public ChangeRelationEvent(IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
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
