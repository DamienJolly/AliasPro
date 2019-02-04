namespace AliasPro.Catalog.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

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
