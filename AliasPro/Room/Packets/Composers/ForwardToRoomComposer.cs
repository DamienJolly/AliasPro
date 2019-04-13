using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class ForwardToRoomComposer : IPacketComposer
    {
        private readonly uint _roomId;

        public ForwardToRoomComposer(uint roomId)
        {
            _roomId = roomId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ForwardToRoomMessageComposer);
            message.WriteInt(_roomId);
            return message;
        }
    }
}
