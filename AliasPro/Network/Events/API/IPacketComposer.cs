namespace AliasPro.Network.Events
{
    using Protocol;

    public interface IPacketComposer
    {
        ServerPacket Compose();
    }
}
