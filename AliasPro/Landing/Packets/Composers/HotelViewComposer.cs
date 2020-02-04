using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Landing.Packets.Composers
{
	public class HotelViewComposer : IMessageComposer
	{
		public ServerMessage Compose() =>
			new ServerMessage(Outgoing.HotelViewMessageComposer);
	}
}
