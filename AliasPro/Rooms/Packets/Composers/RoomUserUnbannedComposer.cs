using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomUserUnbannedComposer : IMessageComposer
    {
        private readonly int _roomId;
        private readonly int _playerId;

        public RoomUserUnbannedComposer(int roomId, int playerId)
        {
            _roomId = roomId;
            _playerId = playerId;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomUserUnbannedMessageComposer);
            message.WriteInt(_roomId);
            message.WriteInt(_playerId);
            return message;
        }
    }
}
