using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomRightsComposer : IPacketComposer
    {
        private readonly int _rightLevel;

        public RoomRightsComposer(int rightLevel)
        {
            _rightLevel = rightLevel;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomRightsMessageComposer);
            message.WriteInt(_rightLevel);
            return message;
        }
    }
}
