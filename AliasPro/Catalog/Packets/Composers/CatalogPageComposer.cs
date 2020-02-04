using AliasPro.API.Catalog.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Catalog.Packets.Composers
{
    public class CatalogPageComposer : IMessageComposer
    {
        private readonly ICatalogPage _catalogPage;
        private readonly string _mode;

        public CatalogPageComposer(ICatalogPage catalogPage, string mode)
        {
            _catalogPage = catalogPage;
            _mode = mode;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.CatalogPageMessageComposer);
            message.WriteInt(_catalogPage.Id);
            message.WriteString(_mode);
            _catalogPage.Layout.Compose(message);

            message.WriteInt(_catalogPage.Items.Count);
            foreach (ICatalogItem catalogitem in _catalogPage.Items.Values)
            {
                catalogitem.Compose(message);
            }

            message.WriteInt(0);
            message.WriteBoolean(false);
            return message;
        }
    }
}
