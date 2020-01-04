using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomAddRightsListComposer : IPacketComposer
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

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomAddRightsListMessageComposer);
            message.WriteInt(_roomId);
            message.WriteInt(_playerId);
            message.WriteString(_playerName);
            return message;
        }
    }
}
