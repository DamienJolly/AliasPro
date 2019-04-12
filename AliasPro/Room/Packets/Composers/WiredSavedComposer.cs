using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Room.Packets.Composers
{
    public class WiredSavedComposer : IPacketComposer
    {
        public ServerPacket Compose() =>
            new ServerPacket(Outgoing.WiredSavedMessageComposer); 
    }
}
