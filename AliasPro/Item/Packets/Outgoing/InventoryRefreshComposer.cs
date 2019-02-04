namespace AliasPro.Item.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class InventoryRefreshComposer : IPacketComposer
    {
        public ServerPacket Compose() =>
            new ServerPacket(Outgoing.InventoryRefreshMessageComposer);
    }
}
