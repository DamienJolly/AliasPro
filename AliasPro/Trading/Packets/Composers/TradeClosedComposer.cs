using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Trading.Packets.Composers
{
    public class TradeClosedComposer : IPacketComposer
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

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.TradeClosedMessageComposer);
            message.WriteInt(_playerId);
			message.WriteInt(_code);
			return message;
        }
    }
}
