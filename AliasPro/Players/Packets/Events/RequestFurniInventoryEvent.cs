using AliasPro.API.Items;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;
using AliasPro.Sessions;

namespace AliasPro.Players.Packets.Events
{
    public class RequestFurniInventoryEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestFurniInventoryMessageEvent;

        private readonly IItemController _itemController;

        public RequestFurniInventoryEvent(IItemController itemController)
        {
            _itemController = itemController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            
            await session.SendPacketAsync(new FurniListComposer(session.Player.Inventory.Items));
        }
    }
}
