using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Trading.Packets.Composers
{
    public class TradeClosedComposer : IMessageComposer
    {
		public static readonly int USER_CANCEL_TRADE = 0;
		public static readonly int ITEMS_NOT_FOUND = 1;

		private readonly int _code;
		private readonly uint _playerId;

        public TradeClosedComposer(int code, uint playerId)
        {
			_code = code;
			_playerId = playerId;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.TradeClosedMessageComposer);
            message.WriteInt((int)_playerId);
			message.WriteInt(_code);
			return message;
        }
    }
}
