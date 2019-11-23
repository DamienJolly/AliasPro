using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;

namespace AliasPro.Players.Packets.Events
{
    public class RequestBotInventoryEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestBotInventoryMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            
            await session.SendPacketAsync(new InventoryBotsComposer(session.Player.Inventory.Bots));
        }
    }
}
