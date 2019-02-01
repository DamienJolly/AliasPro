namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class NavigatorMetaDataParserComposer : ServerPacket
    {
        public NavigatorMetaDataParserComposer()
            : base(Outgoing.NavigatorMetaDataParserMessageComposer)
        {
            WriteInt(4);
            WriteString("official_view");
            WriteInt(0);
            WriteString("hotel_view");
            WriteInt(0);
            WriteString("roomads_view");
            WriteInt(0);
            WriteString("myworld_view");
            WriteInt(0);
        }
    }
}
