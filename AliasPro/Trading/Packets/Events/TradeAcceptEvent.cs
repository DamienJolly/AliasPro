using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.API.Trading.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Trading.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Trading.Packets.Events
{
	public class TradeAcceptEvent : IMessageEvent
	{
		public short Id { get; } = Incoming.TradeAcceptMessageEvent;

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
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
			await trade.SendPacketAsync(new TradeAcceptedComposer(player));

			if (trade.Accepted)
				await trade.SendPacketAsync(new TradingWaitingConfirmComposer());
		}
	}
}

