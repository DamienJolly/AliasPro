using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.API.Trading.Models;
using AliasPro.Trading.Packets.Composers;
using System.Threading.Tasks;
using AliasPro.Rooms.Entities;
using AliasPro.API.Items.Models;
using AliasPro.Items.Packets.Composers;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Trading.Packets.Events
{
	public class TradeConfirmEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.TradeConfirmMessageEvent;

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			IRoom room = session.CurrentRoom;
			if (room == null) return;

			ITrade trade = session.Entity.Trade;
			if (trade == null) return;

			if (!trade.TryGetPlayer(session.Entity.Id, out ITradePlayer player))
				return;

			if (!player.Accepted) return;

			player.Confirmed = true;
			await trade.SendAsync(new TradeAcceptedComposer(player));

			if (trade.Confirmed)
			{
				await HandleItems(trade);
				await trade.EndTrade(true);
			}
		}

		private async Task HandleItems(ITrade trade)
		{
			ITradePlayer userOne = null;
			ITradePlayer userTwo = null;

			foreach (ITradePlayer target in trade.Players)
			{
				if (!(target.Entity is PlayerEntity playerEntity))
					continue;

				foreach (IItem item in target.OfferedItems.Values)
				{
					if (playerEntity.Player.Inventory.Items.Contains(item))
						continue;

					await trade.SendAsync(new TradeClosedComposer(TradeClosedComposer.ITEMS_NOT_FOUND, target.playerId));
					return;
				}

				if (userOne == null)
					userOne = target;
				else
					userTwo = target;
			}

			//int logId = RoomTradingDatabase.CreateTradeLog(userOne, userTwo);

			foreach (IItem item in userOne.OfferedItems.Values)
			{
				if (!(userOne.Entity is PlayerEntity playerEntity))
					continue;

				IList<int> itemIds = new List<int>();

				//RoomTradingDatabase.LogTradeItem(logId, userOne.User.Player.Id, item.Id);
				item.PlayerId = playerEntity.Player.Id;
				item.PlayerUsername = playerEntity.Player.Username;

				if (!(userTwo.Entity is PlayerEntity targetEntity))
					continue;

				targetEntity.Player.Inventory.TryAddItem(item);
				playerEntity.Player.Inventory.RemoveItem(item.Id);

				await targetEntity.Player.Session.SendPacketAsync(new AddPlayerItemsComposer(1, userOne.OfferedItems.Values.Select(c => (int)c.Id).Distinct().ToList()));
			}

			foreach (IItem item in userTwo.OfferedItems.Values)
			{
				if (!(userTwo.Entity is PlayerEntity playerEntity))
					continue;

				//RoomTradingDatabase.LogTradeItem(logId, userOne.User.Player.Id, item.Id);
				item.PlayerId = playerEntity.Player.Id;
				item.PlayerUsername = playerEntity.Player.Username;

				if (!(userOne.Entity is PlayerEntity targetEntity))
					continue;

				targetEntity.Player.Inventory.TryAddItem(item);
				playerEntity.Player.Inventory.RemoveItem(item.Id);

				await targetEntity.Player.Session.SendPacketAsync(new AddPlayerItemsComposer(1, userTwo.OfferedItems.Values.Select(c => (int)c.Id).Distinct().ToList()));
			}

			await trade.SendAsync(new InventoryRefreshComposer());
		}
	}
}

