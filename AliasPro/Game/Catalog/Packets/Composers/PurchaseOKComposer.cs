using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Packets.Composers
{
    public class PurchaseOKComposer : IMessageComposer
    {
        private readonly CatalogItem catalogItem;

        public PurchaseOKComposer(CatalogItem catalogItem)
        {
            this.catalogItem = catalogItem;
        }

		public PurchaseOKComposer()
		{
			this.catalogItem = null;
		}

		public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.PurchaseOKMessageComposer);
			if (catalogItem != null)
				catalogItem.Compose(message);
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
