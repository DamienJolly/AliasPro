using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Trading.Packets.Composers
{
    public class TradingWaitingConfirmComposer : IPacketComposer
    {
		public ServerPacket Compose() =>
			new ServerPacket(Outgoing.TradingWaitingConfirmMessageComposer);
    }
}
