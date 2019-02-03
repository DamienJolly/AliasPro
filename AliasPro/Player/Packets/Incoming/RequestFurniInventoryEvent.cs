using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;
    using Item;
    using Item.Models;

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
            IDictionary<uint, IItem> items;
            if (session.Player.Inventory == null)
            {
                items = await _itemController.GetItemsForPlayerAsync(session.Player.Id);

                session.Player.Inventory = new PlayerInventory(session, items);
            }

            items = session.Player.Inventory.Items;

            await session.SendPacketAsync(new FurniListComposer(items.Values));
        }
    }
}
