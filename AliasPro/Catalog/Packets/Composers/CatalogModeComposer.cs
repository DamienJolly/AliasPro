using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Catalog.Packets.Composers
{
    public class CatalogModeComposer : IMessageComposer
    {
        private readonly int _mode;

        public CatalogModeComposer(int mode)
        {
            _mode = mode;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.CatalogModeMessageComposer);
            message.WriteInt(_mode);
            return message;
        }
    }
}
