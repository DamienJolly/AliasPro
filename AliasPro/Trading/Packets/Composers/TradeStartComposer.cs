using AliasPro.API.Trading.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Trading.Packets.Composers
{
    public class TradeStartComposer : IMessageComposer
    {
		private readonly ITrade _trade;

        public TradeStartComposer(ITrade trade)
        {
			_trade = trade;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.TradeStartMessageComposer);
			foreach (ITradePlayer player in _trade.Players)
			{
				message.WriteInt((int)player.playerId);
				message.WriteInt(1); //dunno?
			}
			return message;
        }
    }
}
