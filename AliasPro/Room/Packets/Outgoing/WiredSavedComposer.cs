namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class WiredSavedComposer : IPacketComposer
    {
        public ServerPacket Compose() =>
            new ServerPacket(Outgoing.WiredSavedMessageComposer); 
    }
}
