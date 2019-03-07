using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;

    public class RequestBadgeInventoryEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestBadgeInventoryMessageEvent;
        
        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            if (session.Player.Badge == null) return;

            await session.SendPacketAsync(new InventoryBadgesComposer(session.Player.Badge.Badges, session.Player.Badge.WearableBadges));
        }
    }
}
