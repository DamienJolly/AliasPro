namespace AliasPro.Catalog.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class AlertLimitedSoldOutComposer : IPacketComposer
    {
        public ServerPacket Compose() =>
            new ServerPacket(Outgoing.AlertLimitedSoldOutMessageComposer);
    }
}
