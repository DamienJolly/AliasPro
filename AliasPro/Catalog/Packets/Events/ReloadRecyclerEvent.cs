using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Catalog.Packets.Events
{
    public class ReloadRecyclerEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ReloadRecyclerMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new ReloadRecyclerComposer());
        }
    }
}
