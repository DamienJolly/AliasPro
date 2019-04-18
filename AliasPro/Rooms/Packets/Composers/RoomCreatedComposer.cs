using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomCreatedComposer : IPacketComposer
    {
        private readonly int _roomId;
        private readonly string _roomName;
        
        public RoomCreatedComposer(int roomId, string roomName)
        {
            _roomId = roomId;
            _roomName = roomName;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomCreatedMessageComposer);
            message.WriteInt(_roomId);
            message.WriteString(_roomName);
            return message;
        }
    }
}
