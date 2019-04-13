using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomCreatedComposer : IPacketComposer
    {
        private readonly IRoomData _roomData;
        
        public RoomCreatedComposer(IRoomData roomData)
        {
            _roomData = roomData;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomCreatedMessageComposer);
            message.WriteInt(_roomData.Id);
            message.WriteString(_roomData.Name);
            return message;
        }
    }
}
