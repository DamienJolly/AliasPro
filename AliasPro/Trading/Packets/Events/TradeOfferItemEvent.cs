using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.API.Trading.Models;
using AliasPro.Trading.Packets.Composers;
using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using System.Threading.Tasks;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Trading.Packets.Events
{
	public class TradeOfferItemEvent : IMessageEvent
	{
		public short Id { get; } = Incoming.TradeOfferItemMessageEvent;

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
		{
			IRoom room = session.CurrentRoom;
			if (room == null) return;

			ITrade trade = session.Entity.Trade;
			if (trade == null) return;

			if (!trade.TryGetPlayer(session.Entity.Id, out ITradePlayer player))
				return;

			int itemId = clientPacket.ReadInt();

			if (!session.Player.Inventory.TryGetItem((uint)itemId, out IItem item))
				return;

			if (!player.TryAddItem(item))
				return;

			await trade.SendPacketAsync(new TradeUpdateComposer(trade));
		}
	}
}

