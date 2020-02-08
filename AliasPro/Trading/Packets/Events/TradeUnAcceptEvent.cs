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
	public class TradeUnAcceptEvent : IMessageEvent
	{
		public short Header => Incoming.TradeUnAcceptMessageEvent;

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

			if (!player.Accepted) return;

			player.Accepted = false;
			await trade.SendPacketAsync(new TradeAcceptedComposer(player));
		}
	}
}

