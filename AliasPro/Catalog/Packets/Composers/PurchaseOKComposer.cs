using AliasPro.API.Catalog.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Catalog.Packets.Composers
{
    public class PurchaseOKComposer : IMessageComposer
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

		public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.PurchaseOKMessageComposer);
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
