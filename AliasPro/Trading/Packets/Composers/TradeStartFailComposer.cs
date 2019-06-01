using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Trading.Packets.Composers
{
    public class TradeStartFailComposer : IPacketComposer
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

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.TradeStartFailMessageComposer);
            message.WriteInt(_code);
			message.WriteString(_username);
			return message;
        }
    }
}
