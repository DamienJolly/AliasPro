using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Players.Packets.Composers
{
    public class HomeRoomComposer : IPacketComposer
    {
        private readonly int _roomId;

        public HomeRoomComposer(int roomId)
        {
            _roomId = roomId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.HomeRoomMessageComposer);
            message.WriteInt(_roomId);
            message.WriteInt(_roomId);
            return message;
        }
    }
}
