using AliasPro.API.Catalog;
using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Server;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Events
{
    public class RecycleEvent : IMessageEvent
    {
        public short Header => Incoming.RecycleMessageEvent;

        private readonly ICatalogController _catalogController;
        private readonly IItemController _itemController;
        private readonly IServerController _serverController;

        public RecycleEvent(
            ICatalogController catalogController,
            IItemController itemController,
            IServerController serverController)
        {
            _catalogController = catalogController;
            _itemController = itemController;
            _serverController = serverController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            if (_serverController.GetSetting("ecotron.enabled") == "0")
            {
                await session.SendPacketAsync(new RecyclerCompleteComposer(RecyclerCompleteComposer.RECYCLING_CLOSED));
                return;
            }

            IList<IItem> items = new List<IItem>();

            int count = message.ReadInt();
            for (int i = 0; i < count; i++)
            {
                int itemId = message.ReadInt();
                if (!session.Player.Inventory.TryGetItem((uint)itemId, out IItem item))
                    continue;

                if (!item.ItemData.AllowRecycle)
                    continue;

                items.Add(item);
            }

            if (!int.TryParse(_serverController.GetSetting("ecotron.items.needed"), out int itemsNeeded) || items.Count != itemsNeeded)
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            IItemData rewardItemData = _catalogController.RecyclerPrize;
            if (rewardItemData == null)
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            if (!_itemController.TryGetItemDataByName(_serverController.GetSetting("ecotron.item.name"), out IItemData giftItemData))
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            foreach (IItem item in items)
            {
                session.Player.Inventory.RemoveItem(item.Id);
                await _itemController.RemoveItemAsync(item);
                await session.SendPacketAsync(new RemovePlayerItemComposer(item.Id));
            }

            string extraData = rewardItemData.Id + "\t" +
                DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;

            IItem rewardItem = new Item(giftItemData.Id, session.Player.Id, session.Player.Username, extraData, giftItemData);
            rewardItem.Id = (uint)await _itemController.AddNewItemAsync(rewardItem);

            if (!session.Player.Inventory.TryAddItem(rewardItem))
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            await session.SendPacketAsync(new AddPlayerItemsComposer(1, (int)rewardItem.Id));
            await session.SendPacketAsync(new RecyclerCompleteComposer(RecyclerCompleteComposer.RECYCLING_COMPLETE));
            await session.SendPacketAsync(new InventoryRefreshComposer());
        }
    }
}
