using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;

namespace AliasPro.Players.Packets.Events
{
    public class RequestPetInventoryEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestPetInventoryMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            
            await session.SendPacketAsync(new InventoryPetsComposer(session.Player.Inventory.Pets));
        }
    }
}
