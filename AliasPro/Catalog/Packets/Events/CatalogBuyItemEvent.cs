using AliasPro.API.Badge;
using AliasPro.API.Badges.Models;
using AliasPro.API.Catalog;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Models;
using AliasPro.Players.Packets.Composers;
using System;
using System.Collections.Generic;

namespace AliasPro.Catalog.Packets.Events
{
    public class CatalogBuyItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.CatalogBuyItemMessageEvent;

        private readonly ICatalogController _catalogController;
        private readonly IItemController _itemController;
        private readonly IBadgeController _badgeController;

		public CatalogBuyItemEvent(
			ICatalogController catalogController, 
			IItemController itemController,
			IBadgeController badgeController)
        {
            _catalogController = catalogController;
            _itemController = itemController;
			_badgeController = badgeController;

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

			IDictionary<int, IList<int>> itemsToAdd = new Dictionary<int, IList<int>>();
			//IList<IItem> itemsList = new List<IItem>();
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
						IItem playerItem = null;

						if (itemData.ItemData.InteractionType == ItemInteractionType.BADGE_DISPLAY)
						{
							if (!session.Player.Badge.HasBadge(extraData))
								continue;

							extraData = extraData + ";" + session.Player.Username + ";" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
							playerItem = new Item((uint)itemData.Id, session.Player.Id, extraData, itemData.ItemData);
						}
						else if (itemData.ItemData.Type == "b")
						{
							if (!_badgeController.TryGetBadge(itemData.ItemData.ExtraData, out IBadge badge))
								continue;

							if (session.Player.Badge.HasBadge(badge.Code))
								continue;

							_badgeController.AddPlayerBadge(session.Player, badge.Code);

							if (!itemsToAdd.ContainsKey(4))
								itemsToAdd.Add(4, new List<int>());

							itemsToAdd[4].Add(badge.Id);

						}
						else if (itemData.ItemData.Type == "r")
						{
							if (itemData.BotData == null)
								continue;

							IPlayerBot playerBot = new PlayerBot(
								0,
								itemData.BotData.Name,
								itemData.BotData.Motto,
								itemData.BotData.Gender,
								itemData.BotData.Figure
							);

							playerBot.Id = await _catalogController.AddNewBotAsync(playerBot, (int)session.Player.Id);

							if (!session.Player.Inventory.TryAddBot(playerBot))
								continue;

							if (!itemsToAdd.ContainsKey(5))
								itemsToAdd.Add(5, new List<int>());

							itemsToAdd[5].Add(playerBot.Id);

						}
						else
						{
							playerItem = new Item((uint)itemData.Id, session.Player.Id, "", itemData.ItemData);
						}

						if (playerItem != null)
						{
							playerItem.Id = (uint)await _itemController.AddNewItemAsync(playerItem);

							if (!session.Player.Inventory.TryAddItem(playerItem))
								continue;

							if (catalogItem.TryGetLimitedNumber(out int limitedNumber))
								await _catalogController.AddLimitedAsync(playerItem.Id, session.Player.Id, limitedNumber);

							if (!itemsToAdd.ContainsKey(1))
								itemsToAdd.Add(1, new List<int>());

							itemsToAdd[1].Add((int)playerItem.Id);
						}
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

			await session.SendPacketAsync(new AddPlayerItemsComposer(itemsToAdd));
            await session.SendPacketAsync(new InventoryRefreshComposer());
			await session.SendPacketAsync(new InventoryBotsComposer(session.Player.Inventory.Bots));
			await session.SendPacketAsync(new PurchaseOKComposer(catalogItem));
        }
    }
}
