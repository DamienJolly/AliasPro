using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Catalog.Packets.Events
{
    public class RequestCatalogIndexEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestCatalogIndexMessageEvent;

        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            // not used?
        }
    }
}
