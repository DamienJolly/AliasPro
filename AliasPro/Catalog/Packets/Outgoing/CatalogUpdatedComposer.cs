namespace AliasPro.Catalog.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class CatalogUpdatedComposer : IPacketComposer
    {
        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.CatalogUpdatedMessageComposer);
            message.WriteBoolean(false);
            return message;
        }
    }
}
