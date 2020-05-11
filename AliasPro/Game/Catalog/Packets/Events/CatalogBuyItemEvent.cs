using AliasPro.API.Permissions;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Badges;
using AliasPro.Game.Badges.Models;
using AliasPro.Game.Catalog.Models;
using AliasPro.Game.Catalog.Packets.Composers;
using AliasPro.Items.Packets.Composers;
using AliasPro.Players.Packets.Composers;
using AliasPro.Utilities;
using System;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Packets.Events
{
    internal class CatalogBuyItemEvent : IMessageEvent
    {
        public short Header => Incoming.CatalogBuyItemMessageEvent;

        private readonly CatalogController catalogController;
        private readonly BadgeController badgeController;

		public CatalogBuyItemEvent(
			CatalogController catalogController, 
			BadgeController badgeController)
        {
            this.catalogController = catalogController;
			this.badgeController = badgeController;
		}

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int pageId = message.ReadInt();
            int itemId = message.ReadInt();
            string extraData = message.ReadString();
            int amount = message.ReadInt();

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

            if (amount <= 0 || amount > 100)
            {
                await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                return;
            }

            if (amount > 1 && !catalogItem.HasOffer)
            {
                await session.SendPacketAsync(new AlertPurchaseUnavailableComposer(AlertPurchaseUnavailableComposer.ILLEGAL));
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

			int totalCredits = TotalCost(catalogItem.Credits, amount);
			int totalPoints = TotalCost(catalogItem.Points, amount);

			if (session.Player.Currency.GetCurrenyAmount(-1) < totalCredits) // not enough credits; player is most likely scripting.
                return;

            if (session.Player.Currency.GetCurrenyAmount(catalogItem.PointsType) < totalPoints) // not enough points; player is most likely scripting.
                return;

            if (catalogItem.hasBadge)
            {
                if (!badgeController.TryGetBadge(catalogItem.Badge, out BadgeData badge))
                {
                    await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                    return;
                }

                if (!string.IsNullOrEmpty(badge.RequiredRight))
                {
                    if (!Program.GetService<IPermissionsController>().HasPermission(session.Player, badge.RequiredRight))
                    {
                        await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                        return;
                    }
                }

                if (session.Player.Badge.HasBadge(badge.Code))
                {
                    await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.ALREADY_HAVE_BADGE));

                    if (catalogItem.Items.Count == 0)
                        return;
                }
                else
                {
                    Program.GetService<IPlayerController>().AddPlayerBadge(session.Player, badge);
                    await session.SendPacketAsync(new AddPlayerItemsComposer(4, badge.Id));
                }
            }

            if (catalogItem.BotId != 0)
                extraData = catalogItem.BotId.ToString();

            foreach (CatalogItemData itemData in catalogItem.Items)
            {
                if (!await catalogController.PurchaseHandler.TryHandlePurchase(session, extraData.Left(150), itemData.ItemData, itemData.Amount * amount))
                {
                    await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
                    return;
                }
            }

            if (totalCredits > 0)
            {
				if (session.Player.Currency.TryGetCurrency(-1, out IPlayerCurrency currency))
				{
					currency.Amount -= catalogItem.Credits;
					await session.SendPacketAsync(new UserCreditsComposer(currency.Amount));
				}
            }

            if (totalPoints > 0)
            {
                if (session.Player.Currency.TryGetCurrency(catalogItem.PointsType, out IPlayerCurrency currency))
                {
                    currency.Amount -= totalPoints;
                    await session.SendPacketAsync(new UserPointsComposer(currency.Amount, -totalPoints, currency.Type));
                }
            }

            await session.SendPacketAsync(new InventoryRefreshComposer());
			await session.SendPacketAsync(new InventoryBotsComposer(session.Player.Inventory.Bots));
			await session.SendPacketAsync(new InventoryPetsComposer(session.Player.Inventory.Pets));
			await session.SendPacketAsync(new PurchaseOKComposer(catalogItem));
        }

		private int TotalCost(int cost, int amount) => 
			amount > 1 ? ((cost * amount) - ((int)Math.Floor((double)amount / 6) * cost)) : cost;
	}
}
