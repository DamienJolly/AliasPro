using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models.Inventory;
    using System.Collections.Generic;
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
            IDictionary<uint, IInventoryItem> items;
            if (session.Player.Inventory == null)
            {
                items = await _itemController.GetItemsForPlayerAsync(session.Player.Id);

                session.Player.Inventory = new Inventory(items);
            }

            items = session.Player.Inventory.Items;

            await session.SendPacketAsync(new FurniListComposer(items.Values));
        }
    }
}
