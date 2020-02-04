using AliasPro.API.Items.Models;
using AliasPro.API.Trading.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Trading.Packets.Composers
{
    public class TradeUpdateComposer : IMessageComposer
    {
		private readonly ITrade _trade;

        public TradeUpdateComposer(ITrade trade)
        {
			_trade = trade;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.TradeUpdateMessageComposer);
			foreach (ITradePlayer player in _trade.Players)
			{
				message.WriteInt((int)player.playerId);
				message.WriteInt(player.OfferedItems.Count);

				foreach (IItem item in player.OfferedItems.Values)
				{
					message.WriteInt((int)item.Id);
					message.WriteString(item.ItemData.Type.ToUpper());
					message.WriteInt((int)item.Id);
					message.WriteInt((int)item.ItemData.SpriteId);
					message.WriteInt(0);
					message.WriteBoolean(item.ItemData.CanStack);
					item.Interaction.Compose(message, true);
					message.WriteInt(0);
					message.WriteInt(0);
					message.WriteInt(0);

					if (item.ItemData.Type.ToUpper() == "S")
						message.WriteInt(0);
				}
				message.WriteInt(player.OfferedItems.Count);
				message.WriteInt(0); //todo: exchange items value
			}
			return message;
        }
    }
}
