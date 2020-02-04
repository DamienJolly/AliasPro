using AliasPro.API.Trading.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Trading.Packets.Composers
{
    public class TradeAcceptedComposer : IMessageComposer
    {
		private readonly ITradePlayer _player;

        public TradeAcceptedComposer(ITradePlayer player)
        {
			_player = player;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.TradeAcceptedMessageComposer);
			message.WriteInt((int)_player.playerId);
			message.WriteInt(_player.Accepted ? 1 : 0);
			return message;
        }
    }
}
