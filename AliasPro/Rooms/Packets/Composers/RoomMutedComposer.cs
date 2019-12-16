using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomMutedComposer : IPacketComposer
    {
        private readonly bool _isMuted;

        public RoomMutedComposer(bool isMuted)
        {
            _isMuted = isMuted;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomMutedMessageComposer);
            message.WriteBoolean(_isMuted);
            return message;
        }
    }
}
