using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog;
using AliasPro.Game.Catalog.Models;
using AliasPro.Game.Catalog.Packets.Composers;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Players.Packets.Composers;
using AliasPro.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Events
{
    public class CatalogBuyGiftItemEvent : IMessageEvent
    {
        public short Header => Incoming.CatalogBuyGiftItemMessageEvent;

        private readonly CatalogController catalogController;
        private readonly IItemController _itemController;
        private readonly IPlayerController _playerController;

		public CatalogBuyGiftItemEvent(
			CatalogController catalogController, 
			IItemController itemController,
			IPlayerController playerController)
        {
            this.catalogController = catalogController;
            _itemController = itemController;
			_playerController = playerController;
		}

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
			int pageId = message.ReadInt();
			int itemId = message.ReadInt();
			string extraData = message.ReadString();
			string username = message.ReadString();
			string note = message.ReadString();
			int spriteId = message.ReadInt();
			int color = message.ReadInt();
			int ribbonId = message.ReadInt();
			bool showName = message.ReadBoolean();

			if (pageId <= 0)
			{
				if (!catalogController.TryGetCatalogItem(itemId, out CatalogItem searchedItem) || searchedItem.OfferId == -1)
				{
					await session.SendPacketAsync(new AlertPurchaseUnavailableComposer(AlertPurchaseUnavailableComposer.ILLEGAL));
					return;
				}

				pageId = searchedItem.PageId;
			}

			if (!catalogController.TryGetCatalogPage(pageId, out CatalogPage page))
			{
				await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
				return;
			}

			if (page.Rank > session.Player.Rank)
			{
				await session.SendPacketAsync(new AlertPurchaseUnavailableComposer(AlertPurchaseUnavailableComposer.ILLEGAL));
				return;
			}

			if (!catalogController.TryGetCatalogItem(itemId, out CatalogItem catalogItem))
			{
				await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
				return;
			}

			if (catalogItem.ClubLevel > session.Player.VipRank)
			{
				await session.SendPacketAsync(new AlertPurchaseUnavailableComposer(AlertPurchaseUnavailableComposer.REQUIRES_CLUB));
				return;
			}

			if (catalogItem.IsLimited)
			{
				await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
				return;
			}

			if (!catalogController.TryGetCatalogGiftData(spriteId, out CatalogGiftPartData gift))
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

			if (catalogItem.Items.Count != 1)
			{
				await session.SendPacketAsync(new AlertPurchaseUnavailableComposer(AlertPurchaseUnavailableComposer.ILLEGAL));
				return;
			}

			CatalogItemData itemData = catalogItem.Items[0];

			if (itemData.ItemData.InteractionType == ItemInteractionType.BADGE_DISPLAY || 
				itemData.ItemData.InteractionType == ItemInteractionType.TELEPORT ||
				(itemData.ItemData.Type.ToLower() != "s" && itemData.ItemData.Type.ToLower() != "i"))
			{
				await session.SendPacketAsync(new AlertPurchaseUnavailableComposer(AlertPurchaseUnavailableComposer.ILLEGAL));
				return;
			}

			int totalCredits = catalogItem.Credits;
			int totalPoints = catalogItem.Points;

			if (totalCredits > 0)
			{
				if (session.Player.Currency.TryGetCurrency(-1, out IPlayerCurrency currency))
				{
					if (totalCredits > currency.Amount)
						return;

					currency.Amount -= totalCredits;
					await session.SendPacketAsync(new UserCreditsComposer(currency.Amount));
				}
			}

			if (totalPoints > 0)
			{
				if (session.Player.Currency.TryGetCurrency(catalogItem.PointsType, out IPlayerCurrency currency))
				{
					if (totalPoints > currency.Amount)
						return;

					currency.Amount -= totalPoints;
					await session.SendPacketAsync(new UserPointsComposer(currency.Amount, -totalPoints, currency.Type));
				}
			}

			extraData = itemData.ItemData.Id + "\t" +
				extraData.Left(150) + "\t" +
				color + "\t" +
				ribbonId + "\t" +
				(showName ? "1" : "0") + "\t" +
				note.Replace("\t", "") + "\t" + 
				session.Player.Username + "\t" +  
				session.Player.Figure;

			IItem giftItem = new Item((uint)gift.ItemId, userId, username, extraData.Left(150), giftItemData);
			giftItem.Id = (uint)await _itemController.AddNewItemAsync(giftItem);

			if (targetPlayer != null)
				targetPlayer.Inventory.TryAddItem(giftItem);

			if (targetPlayer != null)
			{
				await targetPlayer.Session.SendPacketAsync(new AddPlayerItemsComposer(1, (int)giftItem.Id));
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
