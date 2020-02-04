using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Catalog.Packets.Composers
{
	public class ReloadRecyclerComposer : IMessageComposer
	{
		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.ReloadRecyclerMessageComposer);
			message.WriteInt(1);
			message.WriteInt(0);
			return message;
		}
	}
}
