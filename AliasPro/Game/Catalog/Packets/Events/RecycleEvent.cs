using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Packets.Composers;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Packets.Events
{
    public class RecycleEvent : IMessageEvent
    {
        public short Header => Incoming.RecycleMessageEvent;

        private readonly CatalogController catalogController;
        private readonly IItemController itemController;

        public RecycleEvent(
            CatalogController catalogController,
            IItemController itemController)
        {
            this.catalogController = catalogController;
            this.itemController = itemController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            if (Program.ServerSettings.GetBool("ecotron.enabled"))
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

            int itemsNeeded = Program.ServerSettings.GetInt("ecotron.items.needed");
            if (itemsNeeded == 0 || items.Count != itemsNeeded)
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            if (!catalogController.TryGetRecyclerPrize(out IItemData rewardItemData))
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            if (!itemController.TryGetItemDataByName(Program.ServerSettings.GetString("ecotron.item.name"), out IItemData giftItemData))
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            foreach (IItem item in items)
            {
                session.Player.Inventory.RemoveItem(item.Id);
                await itemController.RemoveItemAsync(item);
                await session.SendPacketAsync(new RemovePlayerItemComposer(item.Id));
            }

            string extraData = rewardItemData.Id + "\t" +
                DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;

            IItem rewardItem = new Item(giftItemData.Id, session.Player.Id, session.Player.Username, extraData, giftItemData);
            rewardItem.Id = (uint)await itemController.AddNewItemAsync(rewardItem);

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
