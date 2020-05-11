using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Game.Catalog.Packets.Composers
{
    public class CatalogModeComposer : IMessageComposer
    {
        private readonly int mode;

        public CatalogModeComposer(int mode)
        {
            this.mode = mode;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.CatalogModeMessageComposer);
            message.WriteInt(mode);
            return message;
        }
    }
}
