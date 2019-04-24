﻿using AliasPro.API.Catalog;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;
using System.Collections.Generic;

namespace AliasPro.Catalog.Packets.Events
{
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

        public async void HandleAsync(
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

                if (catalogItem.Items.Count > 1)
                {
                    await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                    return;
                }
            }

            IList<IItem> itemsList = new List<IItem>();
            int totalCredits = 0;
            int totalPoints = 0;

            for (int i = 0; i < amount; i++)
            {
                if (catalogItem.Credits > session.Player.Credits - totalCredits)
                    break;

                if (catalogItem.Points > session.Player.Currency.GetCurrenyAmount(catalogItem.PointsType) - totalPoints)
                    break;

                if (((i + 1) % 6 != 0 && catalogItem.HasOffer) || !catalogItem.HasOffer)
                {
                    totalCredits += catalogItem.Credits;
                    totalPoints += catalogItem.Points;
                }
                
                foreach (ICatalogItemData itemData in catalogItem.Items)
                {
                    for (int k = 0; k < itemData.Amount; k++)
                    {
                        IItem playerItem = page.Layout.HandleItemPurchase(session, itemData, extraData);

                        if (playerItem != null)
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
                if (session.Player.Currency.TryGetCurrency(catalogItem.PointsType, out IPlayerCurrency currency))
                {
                    currency.Amount -= totalPoints;
                    await session.SendPacketAsync(new UserPointsComposer(currency.Amount, -totalPoints, currency.Type));
                }
            }
            
            foreach (IItem item in itemsList)
            {
                if(!session.Player.Inventory.TryAddItem(item))
                    continue;

                item.Id = (uint)await _itemController.AddNewItemAsync(item);

                if (catalogItem.TryGetLimitedNumber(out int limitedNumber))
                    await _catalogController.AddLimitedAsync(item.Id, session.Player.Id, limitedNumber);
            }

            await session.SendPacketAsync(new AddPlayerItemsComposer(itemsList));
            await session.SendPacketAsync(new InventoryRefreshComposer());
            await session.SendPacketAsync(new PurchaseOKComposer(catalogItem));
        }
    }
}
