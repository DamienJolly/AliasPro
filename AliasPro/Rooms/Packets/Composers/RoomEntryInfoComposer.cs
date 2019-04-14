using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomEntryInfoComposer : IPacketComposer
    {
        private readonly uint _roomId;
        private readonly bool _hasRights;

        public RoomEntryInfoComposer(uint roomId, bool hasRights)
        {
            _roomId = roomId;
            _hasRights = hasRights;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomEntryInfoMessageComposer);
            message.WriteInt(_roomId);
            message.WriteBoolean(_hasRights);
            return message;
        }
    }
}
