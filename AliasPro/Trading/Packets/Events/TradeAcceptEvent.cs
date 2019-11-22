using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.API.Trading.Models;
using AliasPro.Trading.Packets.Composers;

namespace AliasPro.Trading.Packets.Events
{
	public class TradeAcceptEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.TradeAcceptMessageEvent;

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			IRoom room = session.CurrentRoom;
			if (room == null) return;

			ITrade trade = session.Entity.Trade;
			if (trade == null) return;

			if (trade.Accepted) return;

			if (!trade.TryGetPlayer(session.Entity.Id, out ITradePlayer player))
				return;

			if (player.Accepted) return;

			player.Accepted = true;
			await trade.SendAsync(new TradeAcceptedComposer(player));

			if (trade.Accepted)
				await trade.SendAsync(new TradingWaitingConfirmComposer());
		}
	}
}

