using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Trading.Packets.Composers
{
    public class TradeStartFailComposer : IMessageComposer
    {
		public static readonly int HOTEL_TRADING_NOT_ALLOWED = 1;
		public static readonly int YOU_TRADING_OFF = 2;
		public static readonly int TARGET_TRADING_OFF = 4;
		public static readonly int ROOM_TRADING_NOT_ALLOWED = 6;
		public static readonly int YOU_ALREADY_TRADING = 7;
		public static readonly int TARGET_ALREADY_TRADING = 8;

		private readonly int _code;
		private readonly string _username;

        public TradeStartFailComposer(int code, string username = "")
        {
			_code = code;
			_username = username;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.TradeStartFailMessageComposer);
            message.WriteInt(_code);
			message.WriteString(_username);
			return message;
        }
    }
}
