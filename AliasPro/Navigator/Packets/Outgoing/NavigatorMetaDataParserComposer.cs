namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

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
