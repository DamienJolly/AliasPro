using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class RequestBadgeInventoryEvent : IMessageEvent
    {
        public short Header => Incoming.RequestBadgeInventoryMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            if (session.Player.Badge == null) return;

            await session.SendPacketAsync(new InventoryBadgesComposer(session.Player.Badge.Badges, session.Player.Badge.WornBadges));
        }
    }
}
