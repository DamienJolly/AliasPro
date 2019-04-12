using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Packets.Composers
{
    public class CatalogModeComposer : IPacketComposer
    {
        private readonly int _mode;

        public CatalogModeComposer(int mode)
        {
            _mode = mode;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.CatalogModeMessageComposer);
            message.WriteInt(_mode);
            return message;
        }
    }
}
