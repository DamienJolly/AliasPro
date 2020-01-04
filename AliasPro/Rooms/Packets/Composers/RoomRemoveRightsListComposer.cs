using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomRemoveRightsListComposer : IPacketComposer
    {
        private readonly int _roomId;
        private readonly int _playerId;

        public RoomRemoveRightsListComposer(int roomId, int playerId)
        {
            _roomId = roomId;
            _playerId = playerId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomRemoveRightsListMessageComposer);
            message.WriteInt(_roomId);
            message.WriteInt(_playerId);
            return message;
        }
    }
}
