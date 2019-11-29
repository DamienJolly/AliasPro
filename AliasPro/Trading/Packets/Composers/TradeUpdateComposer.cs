using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Trading.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Trading.Packets.Composers
{
    public class TradeUpdateComposer : IPacketComposer
    {
		private readonly ITrade _trade;

        public TradeUpdateComposer(ITrade trade)
        {
			_trade = trade;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.TradeUpdateMessageComposer);
			foreach (ITradePlayer player in _trade.Players)
			{
				message.WriteInt(player.playerId);
				message.WriteInt(player.OfferedItems.Count);

				foreach (IItem item in player.OfferedItems.Values)
				{
					message.WriteInt(item.Id);
					message.WriteString(item.ItemData.Type.ToUpper());
					message.WriteInt(item.Id);
					message.WriteInt(item.ItemData.SpriteId);
					message.WriteInt(0);
					message.WriteBoolean(item.ItemData.CanStack);
					//todo: fix
					item.Interaction.Compose(message);
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
