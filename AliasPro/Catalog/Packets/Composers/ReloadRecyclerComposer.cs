using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Packets.Composers
{
	public class ReloadRecyclerComposer : IPacketComposer
	{
		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.ReloadRecyclerMessageComposer);
			message.WriteInt(1);
			message.WriteInt(0);
			return message;
		}
	}
}
