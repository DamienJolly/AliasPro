using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Packets.Composers
{
    public class AlertLimitedSoldOutComposer : IPacketComposer
    {
        public ServerPacket Compose() =>
            new ServerPacket(Outgoing.AlertLimitedSoldOutMessageComposer);
    }
}
