using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.API.Trading.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Trading.Packets.Events
{
	public class TradeCloseEvent : IMessageEvent
	{
		public short Id { get; } = Incoming.TradeCloseMessageEvent;

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
		{
			IRoom room = session.CurrentRoom;
			if (room == null) return;

			ITrade trade = session.Entity.Trade;
			if (trade == null) return;

			await trade.EndTrade(false, session.Player.Id);
		}
	}
}

