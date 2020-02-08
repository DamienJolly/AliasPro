using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.API.Trading.Models;
using AliasPro.Trading.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using System.Threading.Tasks;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Trading.Packets.Events
{
	public class TradeCancelOfferItemEvent : IMessageEvent
	{
		public short Header => Incoming.TradeCancelOfferItemMessageEvent;

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
		{
			IRoom room = session.CurrentRoom;
			if (room == null) return;

			ITrade trade = session.Entity.Trade;
			if (trade == null) return;

			if (trade.Accepted) return;

			if (!trade.TryGetPlayer(session.Entity.Id, out ITradePlayer player))
				return;

			int itemId = message.ReadInt();

			player.RemoveItem((uint)itemId);
			await trade.SendPacketAsync(new TradeUpdateComposer(trade));
		}
	}
}

