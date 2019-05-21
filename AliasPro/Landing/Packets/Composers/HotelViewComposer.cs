using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Landing.Packets.Composers
{
	public class HotelViewComposer : IPacketComposer
	{
		public ServerPacket Compose() =>
			new ServerPacket(Outgoing.HotelViewMessageComposer);
	}
}
