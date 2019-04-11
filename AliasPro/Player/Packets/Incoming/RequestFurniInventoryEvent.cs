using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Item;

    public class RequestFurniInventoryEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestFurniInventoryMessageEvent;

        private readonly IItemController _itemController;

        public RequestFurniInventoryEvent(IItemController itemController)
        {
            _itemController = itemController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            
            await session.SendPacketAsync(new FurniListComposer(session.Player.Inventory.Items));
        }
    }
}
