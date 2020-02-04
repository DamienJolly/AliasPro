using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomAddRightsListComposer : IMessageComposer
    {
        private readonly int _roomId;
        private readonly int _playerId;
        private readonly string _playerName;

        public RoomAddRightsListComposer(int roomId, int playerId, string playerName)
        {
            _roomId = roomId;
            _playerId = playerId;
            _playerName = playerName;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomAddRightsListMessageComposer);
            message.WriteInt(_roomId);
            message.WriteInt(_playerId);
            message.WriteString(_playerName);
            return message;
        }
    }
}
