using AliasPro.API.Catalog.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Packets.Composers
{
    public class PurchaseOKComposer : IPacketComposer
    {
        private readonly ICatalogItem _item;

        public PurchaseOKComposer(ICatalogItem item)
        {
            _item = item;
        }

		public PurchaseOKComposer()
		{
			_item = null;
		}

		public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.PurchaseOKMessageComposer);
			if (_item != null)
				_item.Compose(message);
			else
			{
				message.WriteInt(0);
				message.WriteString("");
				message.WriteBoolean(false);
				message.WriteInt(0);
				message.WriteInt(0);
				message.WriteInt(0);
				message.WriteBoolean(true);
				message.WriteInt(1);
				message.WriteString("s");
				message.WriteInt(0);
				message.WriteString("");
				message.WriteInt(1);
				message.WriteInt(0);
				message.WriteString("");
				message.WriteInt(1);
			}
            return message;
        }
    }
}
