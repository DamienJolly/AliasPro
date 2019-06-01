using AliasPro.API.Network.Events;
using AliasPro.API.Trading.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Trading.Packets.Composers
{
    public class TradeAcceptedComposer : IPacketComposer
    {
		private readonly ITradePlayer _player;

        public TradeAcceptedComposer(ITradePlayer player)
        {
			_player = player;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.TradeAcceptedMessageComposer);
			message.WriteInt(_player.playerId);
			message.WriteInt(_player.Accepted ? 1 : 0);
			return message;
        }
    }
}
