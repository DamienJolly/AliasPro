using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Navigator.Packets.Composers
{
    public class NavigatorMetaDataParserComposer : IMessageComposer
    {
        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.NavigatorMetaDataParserMessageComposer);
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
