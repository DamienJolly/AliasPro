using AliasPro.Network.Protocol;

namespace AliasPro.API.Network.Events
{
    public interface IPacketComposer
    {
        ServerPacket Compose();
    }
}
