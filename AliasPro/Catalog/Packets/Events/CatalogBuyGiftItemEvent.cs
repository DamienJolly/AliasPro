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
using System.Text;

namespace AliasPro.Catalog.Packets.Events
{
    public class CatalogBuyGiftItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.CatalogBuyGiftItemMessageEvent;

        private readonly ICatalogController _catalogController;
        private readonly IItemController _itemController;
        private readonly IBadgeController _badgeController;
        private readonly IPlayerController _playerController;

		public CatalogBuyGiftItemEvent(
			ICatalogController catalogController, 
			IItemController itemController,
			IBadgeController badgeController,
			IPlayerController playerController)
        {
            _catalogController = catalogController;
            _itemController = itemController;
			_badgeController = badgeController;
			_playerController = playerController;
		}

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
			int pageId = clientPacket.ReadInt();
			int itemId = clientPacket.ReadInt();
			string extraData = clientPacket.ReadString();
			string username = clientPacket.ReadString();
			string message = clientPacket.ReadString();
			int spriteId = clientPacket.ReadInt();
			int color = clientPacket.ReadInt();
			int ribbonId = clientPacket.ReadInt();
			bool showName = clientPacket.ReadBool();
			int amount = 1;

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

			if (!_catalogController.TryGetGift(spriteId, out ICatalogGiftPart gift))
			{
				await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
				return;
			}

			if (!_itemController.TryGetItemDataById((uint)gift.ItemId, out IItemData giftItemData))
			{
				await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
				return;
			}

			uint userId;
			if (_playerController.TryGetPlayer(username, out IPlayer targetPlayer))
				userId = targetPlayer.Id;
			else
				userId = await _playerController.TryGetPlayerIdByUsername(username);

			if (userId == 0)
			{
				//session.SendPacketAsync(new GiftReceiverNotFoundComposer());
				return;
			}

			IDictionary<int, IList<int>> itemsData = new Dictionary<int, IList<int>>();
			IList<IItem> itemsToAdd = new List<IItem>();
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
						if (itemData.ItemData.InteractionType == ItemInteractionType.BADGE_DISPLAY)
						{
							continue;
						}
						else if (itemData.ItemData.InteractionType == ItemInteractionType.TROPHY)
						{
							extraData = session.Player.Username + "\t" + DateTime.Now.Day + "\t" + DateTime.Now.Month + "\t" + DateTime.Now.Year + "\t" + extraData;
							itemsToAdd.Add(new Item((uint)itemData.Id, 0, extraData, itemData.ItemData));
						}
						else if (itemData.ItemData.InteractionType == ItemInteractionType.TELEPORT)
						{
							itemsToAdd.Add(new Item((uint)itemData.Id, 0, "", itemData.ItemData));
							itemsToAdd.Add(new Item((uint)itemData.Id, 0, "", itemData.ItemData));

							//todo: re-do teleport links
							//playerItem.ExtraData = playerItemTwo.Id.ToString();
							//playerItemTwo.ExtraData = playerItem.Id.ToString();
						}
						else if (itemData.ItemData.Type == "b")
						{
							if (!_badgeController.TryGetBadge(itemData.ItemData.ExtraData, out IBadge badge))
							{
								await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
								return;
							}

							// todo:
							/*if (session.Player.Badge.HasBadge(badge.Code))
							{
								await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.ALREADY_HAVE_BADGE));
							}*/

							_badgeController.AddPlayerBadge(session.Player, badge.Code);

							if (!itemsData.ContainsKey(4))
								itemsData.Add(4, new List<int>());

							itemsData[4].Add(badge.Id);
						}
						else if (itemData.ItemData.Type == "r")
						{
							if (itemData.BotData == null)
							{
								await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
								return;
							}

							IPlayerBot playerBot = new PlayerBot(
								0,
								itemData.BotData.Name,
								itemData.BotData.Motto,
								itemData.BotData.Gender,
								itemData.BotData.Figure
							);

							playerBot.Id = await _catalogController.AddNewBotAsync(playerBot, (int)userId);

							if (targetPlayer != null)
							{
								if (!targetPlayer.Inventory.TryAddBot(playerBot))
									continue;
							}

							if (!itemsData.ContainsKey(5))
								itemsData.Add(5, new List<int>());

							itemsData[5].Add(playerBot.Id);
						}
						else
						{
							itemsToAdd.Add(new Item((uint)itemData.Id, 0, "", itemData.ItemData));
						}
					}
				}
			}

			StringBuilder giftData = new StringBuilder(itemsToAdd.Count + "\t");

			foreach (IItem item in itemsToAdd)
			{
				item.Id = (uint)await _itemController.AddNewItemAsync(item);
				giftData.Append(item.Id).Append("\t");
			}

			giftData.Append(color).Append("\t").Append(ribbonId).Append("\t").Append(showName ? "1" : "0").Append("\t").Append(message.Replace("\t", "")).Append("\t").Append(session.Player.Username).Append("\t").Append(session.Player.Figure);

			IItem giftItem = new Item((uint)gift.ItemId, userId, giftData.ToString(), giftItemData);
			giftItem.Id = (uint)await _itemController.AddNewItemAsync(giftItem);

			if (targetPlayer != null)
				targetPlayer.Inventory.TryAddItem(giftItem);

			if (!itemsData.ContainsKey(1))
				itemsData.Add(1, new List<int>());

			itemsData[1].Add((int)giftItem.Id);


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

			if (targetPlayer != null)
			{
				await targetPlayer.Session.SendPacketAsync(new AddPlayerItemsComposer(itemsData));
				await targetPlayer.Session.SendPacketAsync(new InventoryRefreshComposer());

				IDictionary<string, string> keys = new Dictionary<string, string>();
				keys.Add("display", "BUBBLE");
				keys.Add("image", "${image.library.url}notifications/gift.gif");
				if (showName)
					keys.Add("message", "You've just recieved a gift from " + session.Player.Username + "!");
				else
					keys.Add("message", "You've just recieved an annoymonus gift!");
				await targetPlayer.Session.SendPacketAsync(new BubbleAlertComposer(BubbleAlertComposer.RECEIVED_BADGE, keys));
			}

			await session.SendPacketAsync(new PurchaseOKComposer(catalogItem));
        }
    }
}
