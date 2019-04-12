using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Navigator.Packets.Composers
{
    public class NavigatorMetaDataParserComposer : IPacketComposer
    {
        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.NavigatorMetaDataParserMessageComposer);
            message.WriteInt(4);
            message.WriteString("official_view");
            message.WriteInt(0);
            message.WriteString("hotel_view");
            message.WriteInt(0);
            message.WriteString("roomads_view");
            message.WriteInt(0);
            message.WriteString("myworld_view");
            message.WriteInt(0);
            return message;
        }
    }
}
