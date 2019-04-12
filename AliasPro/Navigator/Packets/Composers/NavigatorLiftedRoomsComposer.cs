using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Navigator.Packets.Composers
{
    public class NavigatorLiftedRoomsComposer : IPacketComposer
    {
        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.NavigatorLiftedRoomsMessageComposer);
            message.WriteInt(0);
            return message;
        }
    }
}
