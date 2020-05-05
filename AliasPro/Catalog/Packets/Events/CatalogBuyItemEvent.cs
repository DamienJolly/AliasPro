using AliasPro.API.Catalog;
using AliasPro.API.Catalog.Models;
using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Permissions;
using AliasPro.API.Pets;
using AliasPro.API.Pets.Models;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Badges;
using AliasPro.Game.Badges.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Players.Models;
using AliasPro.Players.Packets.Composers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Events
{
    internal class CatalogBuyItemEvent : IMessageEvent
    {
        public short Header => Incoming.CatalogBuyItemMessageEvent;

        private readonly ICatalogController _catalogController;
        private readonly IItemController _itemController;
        private readonly BadgeController badgeController;
        private readonly IPetController _petController;

		public CatalogBuyItemEvent(
			ICatalogController catalogController, 
			IItemController itemController,
			BadgeController badgeController,
			IPetController petController)
        {
            _catalogController = catalogController;
            _itemController = itemController;
			this.badgeController = badgeController;
			_petController = petController;
		}

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int pageId = message.ReadInt();
            int itemId = message.ReadInt();
            string extraData = message.ReadString();
            int amount = message.ReadInt();

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

			IDictionary<int, IList<int>> itemsData = new Dictionary<int, IList<int>>();
			IList<IItem> itemsToAdd = new List<IItem>();
			int totalCredits = 0;
            int totalPoints = 0;

            for (int i = 0; i < amount; i++)
            {
				if (catalogItem.Credits > session.Player.Currency.GetCurrenyAmount(-1) - totalCredits)
				{
					return;
				}

				if (catalogItem.Points > session.Player.Currency.GetCurrenyAmount(catalogItem.PointsType) - totalPoints)
				{
					return;
				}

                if (((i + 1) % 6 != 0 && catalogItem.HasOffer) || !catalogItem.HasOffer)
                {
                    totalCredits += catalogItem.Credits;
                    totalPoints += catalogItem.Points;
                }
                
                foreach (ICatalogItemData itemData in catalogItem.Items)
                {
                    for (int k = 0; k < itemData.Amount; k++)
                    {
						switch (itemData.ItemData.Type.ToLower())
						{
							default:
							case "s":
							case "i":
								{
									if (itemData.ItemData.InteractionType == ItemInteractionType.BADGE_DISPLAY)
									{
										if (!session.Player.Badge.HasBadge(extraData))
										{
											await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
											return;
										}

										if (extraData.Length > 150)
											extraData = extraData.Substring(0, 150);

										extraData = extraData + ";" + session.Player.Username + ";" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
										itemsToAdd.Add(new Item((uint)itemData.Id, session.Player.Id, session.Player.Username, extraData, itemData.ItemData));
									}
									else if (itemData.ItemData.InteractionType == ItemInteractionType.TROPHY)
									{
										extraData = session.Player.Username + "\t" + DateTime.Now.Day + "\t" + DateTime.Now.Month + "\t" + DateTime.Now.Year + "\t" + extraData;
										itemsToAdd.Add(new Item((uint)itemData.Id, session.Player.Id, session.Player.Username, extraData, itemData.ItemData));
									}
									else if (itemData.ItemData.InteractionType == ItemInteractionType.TELEPORT)
									{
										itemsToAdd.Add(new Item((uint)itemData.Id, session.Player.Id, session.Player.Username, "", itemData.ItemData));
										itemsToAdd.Add(new Item((uint)itemData.Id, session.Player.Id, session.Player.Username, "", itemData.ItemData));
									}
									else
									{
										itemsToAdd.Add(new Item((uint)itemData.Id, session.Player.Id, session.Player.Username, extraData, itemData.ItemData));
									}
									break;
								}
							case "b":
								{
									if (!badgeController.TryGetBadge(itemData.ItemData.ExtraData, out BadgeData badge))
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

										if (catalogItem.Items.Count == 1)
											return;
									}
									else
									{
										Program.GetService<IPlayerController>().AddPlayerBadge(session.Player, badge);

										if (!itemsData.ContainsKey(4))
											itemsData.Add(4, new List<int>());

										itemsData[4].Add(badge.Id);
									}
									break;
								}
							case "p":
								{
									string[] data = extraData.Split("\n");

									if (data.Length < 3)
									{
										await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
										return;
									}

									if (!_petController.CheckPetName(data[0]))
									{
										await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
										return;
									}

									if (!_petController.TryGetPetData(itemData.ItemData.Name, out IPetData petData))
									{
										await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
										return;
									}

									IPlayerPet playerPet = new PlayerPet(
										0,
										data[0],
										petData.Type,
										int.Parse(data[1]),
										data[2]
									);

									playerPet.Id = await _catalogController.AddNewPetAsync(playerPet, (int)session.Player.Id);
									session.Player.Inventory.TryAddPet(playerPet);

									if (!itemsData.ContainsKey(3))
										itemsData.Add(3, new List<int>());

									itemsData[3].Add(playerPet.Id);
									break;
								}
							case "r":
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

									playerBot.Id = await _catalogController.AddNewBotAsync(playerBot, (int)session.Player.Id);
									session.Player.Inventory.TryAddBot(playerBot);

									if (!itemsData.ContainsKey(5))
										itemsData.Add(5, new List<int>());

									itemsData[5].Add(playerBot.Id);
									break;
								}
						}
					}
                }
            }

			IItem teleport = null;
			foreach (IItem item in itemsToAdd)
			{
				item.Id = (uint)await _itemController.AddNewItemAsync(item);

				if (!itemsData.ContainsKey(1))
					itemsData.Add(1, new List<int>());

				itemsData[1].Add((int)item.Id);

				if (item.ItemData.InteractionType == ItemInteractionType.TELEPORT & teleport == null)
				{
					teleport = item;
					continue;
				}

				if (teleport != null)
				{
					teleport.ExtraData = item.Id.ToString();
					item.ExtraData = teleport.Id.ToString();

					await _itemController.UpdatePlayerItemsAsync(new List<IItem> { teleport, item });
					session.Player.Inventory.TryAddItem(teleport);
					teleport = null;
				}

				session.Player.Inventory.TryAddItem(item);
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

			await session.SendPacketAsync(new AddPlayerItemsComposer(itemsData));
            await session.SendPacketAsync(new InventoryRefreshComposer());
			await session.SendPacketAsync(new InventoryBotsComposer(session.Player.Inventory.Bots));
			await session.SendPacketAsync(new InventoryPetsComposer(session.Player.Inventory.Pets));
			await session.SendPacketAsync(new PurchaseOKComposer(catalogItem));
        }
    }
}
