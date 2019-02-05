using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Catalog.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Models;
    using Sessions;
    using Item.Models;
    using Player.Packets.Outgoing;
    using Item;
    using AliasPro.Item.Packets.Outgoing;

    public class CatalogBuyItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.CatalogBuyItemMessageEvent;

        private readonly ICatalogController _catalogController;
        private readonly IItemController _itemController;

        public CatalogBuyItemEvent(ICatalogController catalogController, IItemController itemController)
        {
            _catalogController = catalogController;
            _itemController = itemController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int pageId = clientPacket.ReadInt();
            int itemId = clientPacket.ReadInt();
            string extraData = clientPacket.ReadString();
            int amount = clientPacket.ReadInt();

            if (!_catalogController.TryGetCatalogPage(pageId, out ICatalogPage page))
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            if (page.Rank > session.Player.Rank)
            {
                await session.SendPacketAsync(new AlertPurchaseUnavailableComposer(AlertPurchaseUnavailableComposer.ILLEGAL));
                return;
            }
            
            if (!page.TryGetCatalogItem(itemId, out ICatalogItem catalogItem))
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            //todo: player club level
            if (catalogItem.ClubLevel > 1)
            {
                await session.SendPacketAsync(new AlertPurchaseUnavailableComposer(AlertPurchaseUnavailableComposer.REQUIRES_CLUB));
                return;
            }

            if (amount <= 0 || amount > 100)
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            if (amount > 1 && !catalogItem.HasOffer)
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseUnavailableComposer.ILLEGAL));
                return;
            }

            if (catalogItem.IsLimited)
            {
                amount = 1;
                if (catalogItem.LimitedNumbers.Count <= 0)
                {
                    await session.SendPacketAsync(new AlertLimitedSoldOutComposer());
                    return;
                }
            }

            IList<IItem> itemsList = new List<IItem>();
            int totalCredits = 0;
            int totalPoints = 0;

            for (int i = 0; i < amount; i++)
            {
                if (((i + 1) % 6 != 0 && catalogItem.HasOffer) || !catalogItem.HasOffer)
                {
                    totalCredits += catalogItem.Credits;
                    totalPoints += catalogItem.Points;
                }

                foreach (ICatalogItemData item in catalogItem.Items)
                {
                    for (int k = 0; k < item.Amount; k++)
                    {
                        // todo: extradata
                        IItem playerItem = new Item((uint)item.Id, session.Player.Id, "", item.ItemData);
                        itemsList.Add(playerItem);
                    }
                }
            }

            if (totalCredits > 0)
            {
                session.Player.Credits -= totalCredits;
                await session.SendPacketAsync(new UserCreditsComposer(session.Player.Credits));
            }

            if (totalPoints > 0)
            {
                //todo:
                //session.Player.Currency.GetCurrencyType(item.PointsType).Amount -= totalPoints; todo:
                //session.Send(new UserPointsComposer(session.Player.Currency.GetCurrencyType(item.PointsType).Amount, -totalPoints, item.PointsType));
            }

            if (catalogItem.IsLimited)
            {
                //todo:
                //item.AddLimited(catalogItem.GetNumber);
                //await Alias.Server.CatalogManager.AddLimitedAsync(item.Id, catalogItem.GetNumber);
            }

            foreach (IItem item in itemsList)
            {
                item.Id = (uint)await _itemController.AddNewItemAsync(item);
                await session.Player.Inventory.AddItem(item);
            }

            await session.SendPacketAsync(new AddPlayerItemsComposer(itemsList));
            await session.SendPacketAsync(new InventoryRefreshComposer());
            await session.SendPacketAsync(new PurchaseOKComposer(catalogItem));
        }
    }
}
