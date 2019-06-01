using AliasPro.API.Network.Events;
using AliasPro.API.Trading.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Trading.Packets.Composers
{
    public class TradeStartComposer : IPacketComposer
    {
		private readonly ITrade _trade;

        public TradeStartComposer(ITrade trade)
        {
			_trade = trade;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.TradeStartMessageComposer);
			foreach (ITradePlayer player in _trade.Players)
			{
				message.WriteInt(player.Entity.Id);
				message.WriteInt(1); //dunno?
			}
			return message;
        }
    }
}
