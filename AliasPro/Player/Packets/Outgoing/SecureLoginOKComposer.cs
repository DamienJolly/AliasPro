namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class SecureLoginOKComposer : IPacketComposer
    {
        public ServerPacket Compose() =>
            new ServerPacket(Outgoing.SecureLoginOKMessageComposer);
    }
}
