using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Players.Packets.Composers
{
    public class AvailabilityStatusComposer : IPacketComposer
    {
        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AvailabilityStatusMessageComposer);
            message.WriteBoolean(true);
            message.WriteBoolean(false);
            message.WriteBoolean(true);
            return message;
        }
    }
}
