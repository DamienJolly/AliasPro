using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.API.Trading.Models;

namespace AliasPro.Trading.Packets.Events
{
	public class TradeCloseEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.TradeCloseMessageEvent;

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			IRoom room = session.CurrentRoom;
			if (room == null) return;

			ITrade trade = session.Entity.Trade;
			if (trade == null) return;

			await trade.EndTrade(false, session.Player.Id);
		}
	}
}

