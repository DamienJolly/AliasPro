using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;

namespace AliasPro.Players.Packets.Events
{
    public class RequestBadgeInventoryEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestBadgeInventoryMessageEvent;
        
        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            if (session.Player.Badge == null) return;

            await session.SendPacketAsync(new InventoryBadgesComposer(session.Player.Badge.Badges, session.Player.Badge.WornBadges));
        }
    }
}
